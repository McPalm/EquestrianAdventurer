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

	public float hitSkill;
	public float baseDamage;
	public float dodgeSkill;
	public float armor;
	public int baseHP = 10;

	[Space(10)]

	public GameObject skull;

	[SerializeField]
	public MapCharacterEvent EventDeath = new MapCharacterEvent();
	public MapCharacterEvent EventUpdateStats = new MapCharacterEvent();
	public MapCharacterEvent EventKillingBlow = new MapCharacterEvent();

	// Use this for initialization
	void Start ()
	{
		gameObject.layer = LayerMask.NameToLayer("Character");
		GetComponent<CircleCollider2D>().isTrigger = true;
		GetComponent<MapObject>().MapCharacter = this;
		GetComponent<HitPoints>().MaxHealth = baseHP;
		GetComponent<HitPoints>().CurrentHealth = baseHP;
		GetComponent<HitPoints>().EventChangeHealth.AddListener(OnHurt);
		if(GetComponent<Inventory>()) GetComponent<Inventory>().EventChangeEquipment.AddListener(OnChangeEquipment);
		if (EventDeath.GetPersistentEventCount() == 0) EventDeath.AddListener(DefaultDeath);
		// maybe get a health bar and shit
	}

	public bool HostileTowards(MapCharacter other)
	{
		if (alignment == Hostility.player && other.alignment == Hostility.enemies) return true;
		if (alignment == Hostility.enemies && other.alignment == Hostility.player) return true;

		return false;
	}

	public void Melee(MapCharacter target)
	{
		transform.position = (transform.position + target.transform.position) * 0.5f;
		GetComponent<Mobile>().ForceMove((Vector2)GetComponent<MapObject>().RealLocation);

		bool hit = Mathf.Min(Random.value, Random.value) < (hitSkill / (target.dodgeSkill + hitSkill));
		if (hit)
		{
			float damage = (0.75f + Random.value * 0.5f) * baseDamage * 10f / (10f + target.armor);
			if (damage < 1f) damage = 1;
			target.GetComponent<HitPoints>().Hurt(new DamageData().SetDamage((int)damage));
			HurtPool.Instance.DoHurt(target.GetComponent<MapObject>().RealLocation, (int)damage);
			if (target.GetComponent<HitPoints>().CurrentHealth <= 0) EventKillingBlow.Invoke(target);
		}
		else
			CombatTextPool.Instance.PrintAt((Vector3)target.GetComponent<MapObject>().RealLocation + new Vector3(0f, 0.4f), "Dodge", Color.cyan);
	}

	void OnHurt(int current, int max)
	{
		if (current <= 0)
			EventDeath.Invoke(this);
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
		hitSkill = 2 + level;
		baseDamage = 2 + level / 2;
		dodgeSkill = 2 + level;
		armor = 0;
		int hp = baseHP + level * 3 - 3;

		foreach(Equipment e in i.GetEquipped())
		{
			hitSkill += e.hit;
			baseDamage += e.damage;
			dodgeSkill += e.dodge;
			armor += e.armor;
			hp += e.hp;
		}

		GetComponent<HitPoints>().MaxHealth = hp;

		EventUpdateStats.Invoke(this);
	}

	public void SetLevel(int i)
	{
		level = i;
		OnChangeEquipment(GetComponent<Inventory>());
		// GetComponent<HitPoints>().Heal(new DamageData().SetDamage(999)); // yay, max hp on level up!
	}

	[System.Serializable]
	public class MapCharacterEvent : UnityEvent<MapCharacter> { }

	public enum Hostility
	{
		player = 0,
		ally = 1,
		enemies = 2
	}
}
