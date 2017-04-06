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
	Stats stats;
	Stats equip;

	public Stats Stats
	{
		get
		{
			return stats + equip + Auras;
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

	// Use this for initialization
	void Start ()
	{
		gameObject.layer = LayerMask.NameToLayer("Character");
		GetComponent<CircleCollider2D>().isTrigger = true;
		GetComponent<MapObject>().MapCharacter = this;
		GetComponent<HitPoints>().MaxHealth = Stats.hp;
		GetComponent<HitPoints>().CurrentHealth = Stats.hp;
		GetComponent<HitPoints>().EventChangeHealth.AddListener(OnHurt);
		if(GetComponent<Inventory>()) GetComponent<Inventory>().EventChangeEquipment.AddListener(OnChangeEquipment);
		if (EventDeath.GetPersistentEventCount() == 0) EventDeath.AddListener(DefaultDeath);
		// maybe get a health bar and shit
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
			int damage = Stats.DamageVersus(target.Stats, Random.Range(0.75f, 1.25f), Random.value);   // (0.75f + Random.value * 0.5f) * baseDamage * 10f / (10f + target.armor);
			target.GetComponent<HitPoints>().Hurt(new DamageData().SetDamage((int)damage));
			HurtPool.Instance.DoHurt(target.GetComponent<MapObject>().RealLocation, (int)damage);
			if (target.GetComponent<HitPoints>().CurrentHealth <= 0) EventKillingBlow.Invoke(target);
			else NoiseUtility.CauseNoise(Random.Range(4, 10), target.GetComponent<MapObject>().RealLocation);
		}
		else
			CombatTextPool.Instance.PrintAt((Vector3)target.GetComponent<MapObject>().RealLocation + new Vector3(0f, 0.4f), "Dodge", Color.cyan);
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

	int level = 1;

	void OnChangeEquipment(Inventory i)
	{
		equip.hp = level - 1;
		equip.hit = (2 + level) / 3;
		equip.dodge = level / 2;
		equip.armor = 0;
		equip.armorpen = 0;
		equip.damage = level / 4;

		foreach(Equipment e in i.GetEquipped())
		{
			equip += e.stats;
		}
		Refresh();
	}

	public void SetLevel(int i)
	{
		level = i;
		OnChangeEquipment(GetComponent<Inventory>());
		// GetComponent<HitPoints>().Heal(new DamageData().SetDamage(999)); // yay, max hp on level up!
	}

	public void Refresh()
	{
		GetComponent<HitPoints>().MaxHealth = Stats.hp;
		EventUpdateStats.Invoke(this);
	}

	[System.Serializable]
	public class MapCharacterEvent : UnityEvent<MapCharacter> { }
	[System.Serializable]
	public class NoiseEvent : UnityEvent<IntVector2, int> { }

	public enum Hostility
	{
		player = 0,
		ally = 1,
		enemies = 2,
		guard = 3
	}
}
