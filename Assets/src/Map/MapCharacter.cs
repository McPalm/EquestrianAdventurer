﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Mobile))]
[RequireComponent(typeof(HitPoints))]
[RequireComponent(typeof(CircleCollider2D))]
public class MapCharacter : MonoBehaviour
{
	public float hitSkill;
	public float baseDamage;
	public float dodgeSkill;
	public float armor;
	public int baseHP = 10;

	public GameObject skull;

	public UnityEvent EventDeath = new UnityEvent();
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
		// maybe get a health bar and shit
	}

	public void Melee(MapCharacter target)
	{
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
		transform.position = (transform.position + target.transform.position) * 0.5f;
		GetComponent<Mobile>().ForceMove((Vector2)GetComponent<MapObject>().RealLocation);
	}

	void OnHurt(int current, int max)
	{
		if (current <= 0)
		{
			Destroy(gameObject);
			Instantiate(skull, transform.position, Quaternion.identity);
			EventDeath.Invoke();
			GetComponent<Mobile>().enabled = false;
		}
	}

	void OnChangeEquipment(Inventory i)
	{
		hitSkill = 5;
		baseDamage = 3;
		dodgeSkill = 5;
		armor = 0;
		int hp = baseHP;

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

	public class MapCharacterEvent : UnityEvent<MapCharacter> { }
}
