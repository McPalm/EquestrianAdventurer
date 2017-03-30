using UnityEngine;
using System.Collections;

public class Consumable : Item
{
	/// <summary>
	/// The action associated with using the item.
	/// The caller is responsible for consuming the item
	/// </summary>
	public System.Action<GameObject> Use;

	public string description;
	public int turns = 1;

	override public string Tooltip
	{
		get
		{
			return displayName + "(" + value + " bits)" + description;
		}
	}

	public Consumable()
	{
		category = ItemCategory.consumeable;
	}

	override public Item Clone()
	{
		Consumable i = new Consumable();

		i.displayName = displayName;
		i.sprite = sprite;
		i.value = value;
		i.red = red;
		i.green = green;
		i.blue = blue;
		i.category = category;

		i.turns = turns;
		i.description = description;
		i.Use = Use;

		return i;
	}
}
