using UnityEngine;
using System.Collections;

/// <summary>
/// For use in editor only
/// </summary>
public class GroundEquipment : GroundItem
{
	public Equipment equipment;

	new void Start()
	{
		base.Start();	
	}

	public override Item CloneItem()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();

		item.red = sr.color.r;
		item.green = sr.color.g;
		item.blue = sr.color.b;
		item.sprite = sr.sprite;

		item = equipment;
		equipment.category = ItemCategory.equipment;

		return base.CloneItem();
	}
}
