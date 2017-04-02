using UnityEngine;
using System.Collections;

[System.Serializable]
public class Equipment : Item
{
	public EquipmentType slots;

	public Stats stats;

	const float BasePrice = 5f;
	const float priceGrowth = 1.05f;

	public Equipment()
	{
		category = ItemCategory.equipment;
	}

	public override int Value
	{
		get
		{
			value = stats.armor * 3 + stats.dodge * 2 + stats.damage * 3 + stats.hit * 2 + stats.hp - 3 + stats.armorpen * 3;
			value = (int)(BasePrice * (Mathf.Pow(priceGrowth, value)));
			value *= value;
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
			if (stats.armor > 0) s += "\n Armor: " + stats.armor;
			if (stats.damage > 0) s += "\n Damage: " + stats.damage;
			if (stats.armorpen > 0) s += "\n Armor Penetration: " + stats.armorpen;
			if (stats.dodge > 0) s += "\n Dodge: " + stats.dodge;
			if (stats.hp > 0) s += "\n Health: " + stats.hp;
			if (stats.hit > 0) s += "\n Hit: " + stats.hit;
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
		e.slots = slots;

		

		return e;
	}
}
