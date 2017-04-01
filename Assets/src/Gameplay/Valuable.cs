using UnityEngine;
using System.Collections;

[System.Serializable]
public class Valuable : Item
{
	public Valuable()
	{
		category = ItemCategory.valuable;
	}


	public override string Tooltip
	{
		get
		{
			return base.Tooltip + "\nMerchants buy this item at full value!";
		}
	}

	public override Item Clone()
	{
		Valuable i = new Valuable();

		i.displayName = displayName;
		i.sprite = sprite;
		i.value = value;

		i.red = red;
		i.green = green;
		i.blue = blue;

		return i;
	}
}
