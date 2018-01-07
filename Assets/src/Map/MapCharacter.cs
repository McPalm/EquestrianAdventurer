using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Mobile))]
[RequireComponent(typeof(HitPoints))]
[RequireComponent(typeof(CircleCollider2D))]
public class MapCharacter : MonoBehaviour
{
	[Space(10)]

	public Hostility alignment;

	[Space(10)]

	[SerializeField]
	BaseAttributes baseAttributes;
	BaseAttributes equipAttributes;

	[SerializeField]
	Stats stats;
	Stats equip;

    static Gameplay.DiceDeck hostileHitDice;
    static Gameplay.DiceDeck hostileCritDice;
    Gameplay.DiceDeck myHitDice;
    Gameplay.DiceDeck myCritDice;

    public Stats Stats
	{
		get
		{
			return Attributes.Stats + stats + equip + Auras;
		}
	}

	public BaseAttributes Attributes
	{
		get
		{
			return baseAttributes + equipAttributes + Auras2;
		}
	}

	public Stats Auras
	{
		get
		{
			Stats t = new Stats();
			foreach (Aura a in GetComponents<Aura>())
			{
				if (!a.enabled) continue;
				t += a.Stats;
			}
			return t;
		}
	}

	public BaseAttributes Auras2
	{
		get
		{
			BaseAttributes t = new BaseAttributes();
			foreach (Aura a in GetComponents<Aura>())
			{
				if (!a.enabled) continue;
				t += a.Attributes;
			}
			return t;
		}
	}

    /// <summary>
    /// Make ana ttack roll against the target
    /// </summary>
    /// <param name="target">target</param>
    /// <param name="mayCrit">true if the attack may score a critical hit</param>
    /// <param name="weapon">name of the weapon for logging purposes</param>
    /// <returns>0 = miss, 1 = hit, 2 = critical hit</returns>
    public int HitRollVs(MapCharacter target, bool mayCrit = true, string weapon = "")
    {
        int roll = myHitDice.Next();
        bool hit = roll >= Stats.HitNumber(target.Stats);

        // only draw a number if critical hits are possible.
        int critRoll = (Stats.CritNumber(target.Stats) < 21 && mayCrit) ? myCritDice.Next() : Random.Range(1, 21);
        bool crit = critRoll >= Stats.CritNumber(target.Stats);

        if (weapon != "") weapon = " with " + weapon;
        if(!hit)
            Debug.Log(string.Format("{0} attack {1}{2}, and ({3}) {4}!",
                GetComponent<Mobile>().displayName,
                target.GetComponent<Mobile>().displayName,
                weapon,
                roll,
                "Miss"
                ));
        else if(mayCrit)
            Debug.Log(string.Format("{0} attack {1}{2}, and ({3})({4}) {5}!",
                GetComponent<Mobile>().displayName,
                target.GetComponent<Mobile>().displayName,
                weapon,
                roll,
                critRoll,
                (crit) ? "Critical Hit" : "Hit"
                ));
        else
            Debug.Log(string.Format("{0} attack {1}{2}, and ({3}) {4}!",
                GetComponent<Mobile>().displayName,
                target.GetComponent<Mobile>().displayName,
                weapon,
                roll,
                (hit) ? "Hit" : "Miss"
                ));


        return (hit) ? ((crit) ? 2 : 1) : 0;
    }

	[Space(10)]

	public GameObject skull;

	[SerializeField]
	public MapCharacterEvent EventDeath = new MapCharacterEvent();
	public MapCharacterEvent EventUpdateStats = new MapCharacterEvent();
	public MapCharacterEvent EventKillingBlow = new MapCharacterEvent();
	public NoiseEvent EventHearNoise = new NoiseEvent();
	public GameobjectEvent EventHit = new GameobjectEvent();
	public GameobjectEvent EventMiss = new GameobjectEvent();
    public GameobjectEvent EventDodge = new GameobjectEvent();
    public GameobjectEvent EventStruck = new GameobjectEvent();

	// Use this for initialization
	void Start ()
	{
		gameObject.layer = LayerMask.NameToLayer("Character");
		GetComponent<CircleCollider2D>().isTrigger = true;
		GetComponent<MapObject>().MapCharacter = this;
		GetComponent<HitPoints>().MaxHealth = Stats.hp;
		GetComponent<HitPoints>().CurrentHealth = Stats.hp;
		GetComponent<HitPoints>().EventChangeHealth.AddListener(OnHurt);
		GetComponent<HitPoints>().EventBeforeHurt.AddListener(OnHurt);
		if (GetComponent<Inventory>()) GetComponent<Inventory>().EventChangeEquipment.AddListener(OnChangeEquipment);
		if (EventDeath.GetPersistentEventCount() == 0) EventDeath.AddListener(DefaultDeath);
        if (hostileHitDice == null) hostileHitDice = new Gameplay.DiceDeck(20, 5);
        if (hostileCritDice == null) hostileCritDice = new Gameplay.DiceDeck(20, 0);
        myHitDice = (alignment == Hostility.player) ? new Gameplay.DiceDeck(20, 5) : hostileHitDice;
        myCritDice = (alignment == Hostility.player) ? new Gameplay.DiceDeck(20, 5) : hostileCritDice;
        // maybe get a health bar and shit
        Refresh();
	}

	public bool HostileTowards(MapCharacter other)
	{
		if (alignment == Hostility.player && other.alignment == Hostility.enemies) return true;
		if (alignment == Hostility.enemies && other.alignment == Hostility.player) return true;
		if (alignment == Hostility.guard && other.alignment == Hostility.enemies) return true;
		if (alignment == Hostility.enemies && other.alignment == Hostility.guard) return true;

		return false;
	}

	public void Melee(MapCharacter target)
	{
		transform.position = (transform.position + target.transform.position) * 0.5f;
		GetComponent<Mobile>().ForceMove(GetComponent<MapObject>().RealLocation);
        int hitRoll = HitRollVs(target);
        if (hitRoll > 0)
		{
			EventHit.Invoke(target.gameObject);
            target.EventStruck.Invoke(gameObject);
            DamageData data = new DamageData(gameObject)
                .SetDamage(Stats.damage * Random.Range(0.75f, 1.25f), Stats.damage * 0.75f)
                .SetArmorPen(Stats.armorpen)
                .AddType(Stats.damageTypes)
                .SetCritical(hitRoll == 2);
            target.GetComponent<HitPoints>().Hurt(data);
			HurtPool.Instance.DoHurt(target.GetComponent<MapObject>().RealLocation, data.TotalDamage);
			if (data.critical) CombatTextPool.Instance.PrintAt((Vector3)target.GetComponent<MapObject>().RealLocation + new Vector3(0f, 0.4f), "Crit!", new Color(1f, 0.5f, 0f));
			if (target.GetComponent<HitPoints>().CurrentHealth <= 0) EventKillingBlow.Invoke(target);
			else NoiseUtility.CauseNoise(Random.Range(4, 10), target.GetComponent<MapObject>().RealLocation);
		}
		else
		{
			CombatTextPool.Instance.PrintAt((Vector3)target.GetComponent<MapObject>().RealLocation + new Vector3(0f, 0.4f), "Dodge", Color.cyan);
			EventMiss.Invoke(target.gameObject);
            target.EventDodge.Invoke(gameObject);
		}
	}

	void OnHurt(DamageData d)
	{
		if (d.HasAnyType(DamageTypes.physical | DamageTypes.piercing | DamageTypes.slashing | DamageTypes.bludgeoning))
		{
			d.multiplier *= Stats.DamageReduction(d.armorPenetration);
		}
	}

	bool death = false;
	void OnHurt(int current, int max)
	{
		if (current <= 0 && !death)
		{
			death = true;
			EventDeath.Invoke(this);
			foreach (Aura a in GetComponents<Aura>())
				Destroy(a);
			Refresh();
			death = false;
		}
	}

	public void DefaultDeath(MapCharacter o)
	{
		Destroy(gameObject);
		Instantiate(skull, transform.position, Quaternion.identity);
		GetComponent<Mobile>().enabled = false;
	}

	void OnChangeEquipment(Inventory i)
	{
		equip = new Stats();
		equipAttributes = new BaseAttributes();

		foreach (Equipment e in i.GetEquipped())
		{
			equip += e.stats;
			equipAttributes += e.attributes;
		}
		Refresh();
	}

	bool refreshing = false;

	public void Refresh()
	{
		if (!refreshing)
		{
			refreshing = true;
			GetComponent<HitPoints>().MaxHealth = Stats.hp;
			if (GetComponent<StaminaPoints>())
			{
				GetComponent<StaminaPoints>().MaxStamina = Attributes.Endurance + 3;
				GetComponent<StaminaPoints>().StaminaPerTurn = Attributes.RecoveryRate;
			}
			EventUpdateStats.Invoke(this);
			refreshing = false;
		}
	}

	[System.Serializable]
	public class MapCharacterEvent : UnityEvent<MapCharacter> { }
	[System.Serializable]
	public class NoiseEvent : UnityEvent<IntVector2, int> { }
	[System.Serializable]
	public class GameobjectEvent : UnityEvent<GameObject> { }

	public enum Hostility
	{
		player = 0,
		ally = 1,
		enemies = 2,
		guard = 3
	}
}
