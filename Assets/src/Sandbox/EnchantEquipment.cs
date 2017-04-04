﻿using UnityEngine;
using System.Collections;

public static class EnchantEquipment
{
	/// <summary>
	/// Accepts an item and might enchant it..   might
	/// </summary>
	/// <param name="e"></param>
	static public void Enchant(Equipment e)
	{
		if(Random.value < 0.2f) // poor quality
		{
			if(Random.value < 0.5f)
				e.displayName = "Poor " + e.displayName;
			else if (Random.value < 0.5f)
				e.displayName = "Jagged " + e.displayName;
			else if (Random.value < 0.5f)
				e.displayName = "Worn " + e.displayName;
			else if (Random.value < 0.5f)
				e.displayName = "Toy " + e.displayName;
			else
				e.displayName = "Anchient " + e.displayName;
			if (e.slots == EquipmentType.body)
			{
				e.stats.armor = e.stats.armor * 3 / 4;
				e.stats.dodge -= 2;
			}
			else
			{
				e.stats.damage = e.stats.damage * 2 / 3;
				e.stats.hit -= 2;
			}
			e.value /= 2;
			return;
		}
		
		if(Random.value < 0.25f)
		{
			if (e.slots == EquipmentType.body)
				MundaneArmorQuality(e);
			else
				MundaneWeaponQuality(e);
		}
		if(Random.value < 0.1f)
			MagicEnchant(e);
	}

	public static void MundaneWeaponQuality(Equipment e)
	{
		e.value += 10;
		switch (Random.Range(0, 5))
		{
			case 1:
				e.displayName = "Fine " + e.displayName;
				e.stats.hit += 1;
				break;
			case 2:
				e.displayName = "Hefty " + e.displayName;
				e.stats.damage += 1;
				e.stats.hit -= 1;
				break;
			case 3:
				e.displayName = "Serrated " + e.displayName;
				e.stats.armorpen -= 1;
				e.stats.damage += 1;				
				break;
			case 4:
				e.displayName = "Masterwork " + e.displayName;
				e.stats.armorpen += 1;
				e.value += 40;
				break;
		}
	}

	public static void MundaneArmorQuality(Equipment e)
	{
		e.value += 10;
		switch (Random.Range(0, 5))
		{
			case 1:
				e.displayName = "Fine " + e.displayName;
				e.stats.dodge += 1;
				break;
			case 2:
				if (e.stats.armor > 0)
				{
					e.displayName = "Reinforced " + e.displayName;
					e.stats.dodge -= (1 + e.stats.armor / 5);
					e.stats.armor += (1 + e.stats.armor / 5);
				}
				else
				{
					e.displayName = "Armored " + e.displayName;
					e.stats.armor += (1 + e.stats.dodge / 5);
					e.stats.dodge -= (1 + e.stats.dodge / 5);
				}
				break;
			case 3:
				if (e.stats.armor > 0)
				{
					e.displayName = "Spiked " + e.displayName;
					e.stats.damage += (1 + e.stats.armor / 5);
					e.stats.armor -= (1 + e.stats.armor / 5);
				}
				else
				{
					e.displayName = "Lovely " + e.displayName;
					e.stats.dodge -= (1 + e.stats.dodge / 2);
					e.stats.hit += (1 + e.stats.dodge / 2);
				}
				break;
			case 4:
				e.displayName = "Masterwork " + e.displayName;
				e.stats.armor += 1;
				break;
		}
	}

	public static void MagicEnchant(Equipment e)
	{
		int plus = Mathf.Min(Random.Range(1, 6), Random.Range(1, 6));

		e.displayName = e.displayName + " +" + plus;

		if (e.slots == EquipmentType.body)
		{
			e.stats.armor += plus;
			if (e.stats.dodge > e.stats.armor)
			{
				e.stats.dodge += plus;
				e.stats.hp += plus;
			}
			else
			{
				e.stats.dodge += plus / 2;
				e.stats.hp += plus * 3;
			}
		}
		else
		{
			e.stats.hit += plus;
			e.stats.damage += plus;
			e.stats.armorpen += plus;
		}

		e.value += e.enchantCost * plus * plus;
	}
}
