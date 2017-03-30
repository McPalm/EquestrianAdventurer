using UnityEngine;
using System.Collections;

/// <summary>
/// For use in editor only
/// </summary>
public class GroundEquipment : GroundItem
{
	public Equipment equipment;
	public bool randomEnchant = true;

	new void Start()
	{
		base.Start();	
	}

	public override Item CloneItem()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();

		equipment.category = ItemCategory.equipment;

		if (randomEnchant)
		{
			Equipment e = equipment.Clone() as Equipment;
			EnchantEquipment.Enchant(e);
			item = e;
		}
		else
		{
			item = equipment;
		}

		item.red = sr.color.r;
		item.green = sr.color.g;
		item.blue = sr.color.b;
		item.sprite = sr.sprite;

		return base.CloneItem();
	}
}
