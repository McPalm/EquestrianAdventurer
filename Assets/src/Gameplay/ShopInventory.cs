using UnityEngine;
using System.Collections.Generic;

public class ShopInventory : MonoBehaviour
{
	public List<Item> inventory;
	public List<Item> buyBack;

	public GroundItem[] items;


	// Use this for initialization
	void Start ()
	{
		inventory = new List<Item>();
		for(int i = 0; i < items.Length; i++)
		{
			inventory.Add(items[i].item.Clone());
		}
	}

	public void Add(Item i)
	{
		buyBack.Add(i);
	}

	public void Remove(Item i)
	{
		inventory.Remove(i);
		buyBack.Remove(i);
	}
}
