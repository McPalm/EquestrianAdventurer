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
			return baseAttributes + equipAttributes;
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

	[Space(10)]

	public GameObject skull;

	[SerializeField]
	public MapCharacterEvent EventDeath = new MapCharacterEvent();
	public MapCharacterEvent EventUpdateStats = new MapCharacterEvent();
	public MapCharacterEvent EventKillingBlow = new MapCharacterEvent();
	public NoiseEvent EventHearNoise = new NoiseEvent();
	public GameobjectEvent EventHit = new GameobjectEvent();
	public GameobjectEvent EventMiss = new GameobjectEvent();

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

		bool hit = Mathf.Min(Random.value, Random.value) < Stats.HitChance(target.Stats); // (hitSkill / (target.dodgeSkill + hitSkill));
		if (hit)
		{
			EventHit.Invoke(target.gameObject);
			DamageData data = new DamageData(gameObject)
				.SetDamage(Stats.damage * Random.Range(0.75f, 1.25f))
				.SetArmorPen(Stats.armorpen)
				.AddType(Stats.damageTypes)
				.SetCritical(Random.value < Stats.CritChance(target.Stats));
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
		}
	}

	void OnHurt(DamageData d)
	{
		if (d.HasAnyType(DamageTypes.physical | DamageTypes.piercing | DamageTypes.slashing | DamageTypes.bludgeoning)) d.multiplier *= stats.DamageReduction(d.armorPenetration);
	}

	void OnHurt(int current, int max)
	{
		if (current <= 0)
		{
			EventDeath.Invoke(this);
			foreach (Aura a in GetComponents<Aura>())
				Destroy(a);
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

	public void Refresh()
	{
		GetComponent<HitPoints>().MaxHealth = Stats.hp;
		if (GetComponent<StaminaPoints>())
		{
			GetComponent<StaminaPoints>().MaxStamina = Attributes.Endurance + 3;
			GetComponent<StaminaPoints>().StaminaPerTurn = Attributes.Endurance / 20f;
		}
		EventUpdateStats.Invoke(this);
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
