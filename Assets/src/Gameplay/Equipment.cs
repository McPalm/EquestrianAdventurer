using UnityEngine;
using System.Collections;

[System.Serializable]
public class Equipment : Item
{
	public EquipmentType slots;

	public int armor;
	public int dodge;
	public int damage;
	public int hit;
	public int hp;

	public override int Value
	{
		get
		{
			value = armor * armor * 2 + dodge * dodge + damage * damage + hit * hit + hp * hp / 3;
			return value;
		}

		set
		{
			base.Value = value;
		}
	}

	public override string Tooltip
	{
		get
		{
			string s = base.Tooltip;
			if (armor > 0) s += "\n Armor: " + armor;
			if (damage > 0) s += "\n Damage: " + damage;
			if (dodge > 0) s += "\n Dodge: " + dodge;
			if (hp > 0) s += "\n Health: " + hp;
			if (hit > 0) s += "\n Hit: " + hit;
			return s;
		}
	}

	public override Item Clone()
	{
		Equipment e = new Equipment();

		e.displayName = displayName;
		e.sprite = sprite;
		e.value = value;
		e.red = red;
		e.green = green;
		e.blue = blue;

		e.armor = armor;
		e.damage = damage;
		e.dodge = dodge;
		e.hp = hp;
		e.hit = hit;

		return e;
	}
}
