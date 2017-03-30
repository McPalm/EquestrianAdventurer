using UnityEngine;
using System.Collections;

public class Consumeable : Item
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

	public Consumeable()
	{
		category = ItemCategory.consumeable;
	}
}
