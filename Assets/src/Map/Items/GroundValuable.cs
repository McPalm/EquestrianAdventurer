using UnityEngine;
using System.Collections;

public class GroundValuable : GroundItem
{
	public string itemName;
	public int value;
	bool refine = true;

	public override Item CloneItem()
	{
		Valuable v = new Valuable();
		if (refine && Random.value < 0.1f)
		{
			if (Random.value < 0.1f)
			{
				v.value = (int)(value * Random.Range(2f, 10f));
				v.displayName = "Divine " + itemName;
			}
			else
			{
				v.value = (int)(value * Random.Range(1.5f, 2f));
				v.displayName = "Pristine " + itemName;
			}
		}
		else
		{
			v.value = (int)(value * Random.Range(0.9f, 1.1f));
			v.displayName = itemName;
		}
		item = v;
		return base.CloneItem();
	}
}
