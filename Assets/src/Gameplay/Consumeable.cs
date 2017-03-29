using UnityEngine;
using System.Collections;

public class Consumeable : Item
{
	/// <summary>
	/// The action associated with using the item.
	/// The caller is responsible for consuming the item
	/// </summary>
	public System.Action<GameObject> Use;

	public Consumeable()
	{
		category = ItemCategory.consumeable;
	}
}
