using UnityEngine;
using System.Collections;

public class GroundGold : GroundItem
{

	public int minVal = 1;
	public int maxVal = 10;

	public override Item CloneItem()
	{
		item.value = Random.Range(minVal, maxVal + 1);
		item.category = ItemCategory.gold;
		return base.CloneItem();
	}
}
