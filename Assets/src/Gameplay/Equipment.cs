using UnityEngine;
using System.Collections;

[System.Serializable]
public class Equipment : Item
{
	public EquipmentType slots;

	public Stats stats;
	public BaseAttributes attributes;
	public int enchantCost = 100;
	

	public Equipment()
	{
		category = ItemCategory.equipment;
	}

	public override int Value
	{
		get
		{
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
			s += stats.NeatStringSkipEmpty(0) + attributes.NeatStringSkipEmpty(0);
			/*
			if (stats.armor != 0) s += "\n Armor: " + stats.armor;
			if (stats.damage != 0) s += "\n Damage: " + stats.damage;
			if (stats.armorpen != 0) s += "\n Armor Penetration: " + stats.armorpen;
			if (stats.dodge != 0) s += "\n Dodge: " + stats.dodge;
			if (stats.hp != 0) s += "\n Health: " + stats.hp;
			if (stats.hit != 0) s += "\n Hit: " + stats.hit;
			*/
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
		e.category = category;

		e.stats = stats;
		e.attributes = attributes;
		e.slots = slots;
		e.enchantCost = enchantCost;
		

		return e;
	}
}
