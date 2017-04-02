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
			e.displayName = "Poor " + e.displayName;
			if (e.slots == EquipmentType.body)
			{
				e.stats.armor = e.stats.armor * 3 / 4;
				e.stats.dodge -= 2;
			}
			else
			{
				e.stats.damage -= 1;
				e.stats.hit -= 2;
			}
		}
		else if (Random.value < 0.2f) // fine
		{
			e.displayName = "Fine " + e.displayName;
			if (e.slots == EquipmentType.body)
			{
				e.stats.dodge += 1;
			}
			else
			{
				e.stats.hit += 1;
			}
		}

		if(Random.value < 0.07f) // magical
		{
			int plus = Mathf.Min(Random.Range(1, 6), Random.Range(1, 6));

			e.displayName = e.displayName + " +" + plus;

			if (e.slots == EquipmentType.body)
			{
				e.stats.dodge += 1 + plus / 2;
				e.stats.armor += plus;
				e.stats.hp += plus * 2;
			}
			else
			{
				e.stats.hit += plus;
				e.stats.damage += plus;
			}
		}
	}
}
