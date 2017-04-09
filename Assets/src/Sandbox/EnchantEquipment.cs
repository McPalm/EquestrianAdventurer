using UnityEngine;
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
			if (e.slots == EquipmentType.body || e.slots == EquipmentType.hooves)
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
			else if (e.slots == EquipmentType.weapon)
				MundaneWeaponQuality(e);
			else if(e.slots == EquipmentType.trinket)
			{
				if(Random.value < 0.4f) e.displayName = "Quirky " + e.displayName;
				else if (Random.value < 0.66f) e.displayName = "Strange " + e.displayName;
				else if(Random.value < 0.5f) e.displayName = "Peculiar " + e.displayName;
				else e.displayName = "Perplexing " + e.displayName;
				e.attributes += RandomAttribute();
				e.attributes -= RandomAttribute();
				if (Random.value < 0.4f)
				{
					e.attributes += RandomAttribute();
					e.attributes -= RandomAttribute();
					if (Random.value < 0.4f)
					{
						e.attributes += RandomAttribute();
						e.attributes -= RandomAttribute();
						e.value += e.enchantCost;
					}
					e.value += e.enchantCost / 2;
				}
				e.value += e.enchantCost / 4;
			}
			else
			{
				MundaneSecondaryArmorEquality(e);
			}
				
		}
		if(Random.value < 0.1f)
			MagicEnchant(e);
	}

	static public void MundaneSecondaryArmorEquality(Equipment e)
	{
		switch (Random.Range(0, 5))
		{
			case 1:
				e.displayName = "Sturdy " + e.displayName;
				e.stats.armor += 1;
				e.stats.dodge -= 1;
				e.value += 7;
				break;
			case 2:
				e.displayName = "Quirky " + e.displayName;
				e.attributes += RandomAttribute();
				e.attributes -= RandomAttribute();
				e.value += e.enchantCost / 4;
				break;
			case 3:
				e.displayName = "Lithe " + e.displayName;
				e.stats.dodge += 1;
				break;
			case 4:
				if(e.stats.armor > 0)
				{
					e.displayName = "Bulky " + e.displayName;
					e.stats.armor += 2;
					e.stats.dodge -= 1;
					e.attributes -= RandomAttribute();
					e.value += e.enchantCost;
				}
				else
				{
					e.displayName = "Dancers " + e.displayName;
					e.attributes.Agility += 2;
					e.stats.hit -= 1;
					e.attributes -= RandomAttribute();
					e.value += e.enchantCost;
				}
				
				break;
		}
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
					e.stats.hit += (1 + e.stats.dodge / 2);
					e.stats.dodge -= (1 + e.stats.dodge / 2);
					
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
		int plus = Mathf.Min(Random.Range(1, 7), Random.Range(1, 7));

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
		else if(e.slots == EquipmentType.weapon)
		{
			e.stats.hit += plus;
			e.stats.damage += plus;
			e.stats.armorpen += plus;
		}
		else if(e.slots == EquipmentType.hooves)
		{
			if(e.stats.armor > e.stats.dodge)
			{
				e.stats.armor += plus;
				e.stats.dodge += plus / 2;
			}
			else
			{
				e.stats.dodge += plus;
				e.stats.armor += plus / 2;
			}
			for (int i = 0; i < (1 + plus) / 2; i++)
			{
				e.attributes += RandomAttribute();
			}
		}
		else if (e.slots == EquipmentType.head)
		{
			e.stats.armor += plus / 2;
			e.stats.critAvoid += plus;
			
			for (int i = 0; i < (1 + plus) / 2; i++)
			{
				e.attributes += RandomAttribute();
			}
		}
		else
		{
			for(int i = 0; i < plus; i++)
			{
				e.attributes += RandomAttribute();
			}
		}
		int epic = 0;
		if (Random.value < 0.2f)
		{
			epic = Random.Range(1, 4);
			for (int i = 0; i < epic; i++)
				e.attributes += RandomAttribute();
		}

		plus += epic;
		e.value += e.enchantCost * plus * plus;
		if (plus > 7)
			e.displayName = "Harmonys " + e.displayName;
		else if (plus > 5)
			e.displayName = "Alicorns " + e.displayName;
	}


	static public BaseAttributes RandomAttribute()
	{
		BaseAttributes r = new BaseAttributes();
		switch(Random.Range(0, 5))
		{
			case 0: r.Strenght = 1; break;
			case 1: r.Dexterity = 1; break;
			case 2: r.Agility = 1; break;
			case 3: r.Endurance = 1; break;
			case 4: r.Tenacity = 1; break;
		}
		return r;
	}
}
