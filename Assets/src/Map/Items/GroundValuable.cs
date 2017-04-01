using UnityEngine;
using System.Collections;

public class GroundValuable : GroundItem
{
	public string itemName;
	public int value;

	public override Item CloneItem()
	{
		Valuable v = new Valuable();
		v.value = value;
		v.displayName = itemName;
		item = v;
		return base.CloneItem();
	}
}
