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
				e.armor = e.armor * 3 / 4;
				e.dodge -= 2;
			}
			else
			{
				e.damage -= 1;
				e.hit -= 2;
			}
		}
		else if (Random.value < 0.2f) // fine
		{
			e.displayName = "Fine " + e.displayName;
			if (e.slots == EquipmentType.body)
			{
				e.dodge += 1;
			}
			else
			{
				e.hit += 1;
			}
		}

		if(Random.value < 0.07f) // magical
		{
			int plus = Mathf.Min(Random.Range(1, 6), Random.Range(1, 6));

			e.displayName = e.displayName + " +" + plus;

			if (e.slots == EquipmentType.body)
			{
				e.dodge += 1 + plus / 2;
				e.armor += plus;
				e.hp += plus * 2;
			}
			else
			{
				e.hit += plus;
				e.damage += plus;
			}
		}
	}
}
