using UnityEngine;
using System.Collections.Generic;

public class ShopInventory : MonoBehaviour
{
	public List<Item> inventory;
	public List<Item> buyBack;

	public GroundItem[] items;
	public GroundEquipment[] equipment;



	// Use this for initialization
	void Awake()
	{
		inventory = new List<Item>();
		for(int i = 0; i < items.Length; i++)
		{
			inventory.Add(items[i].item.Clone());
		}
		for (int i = 0; i < equipment.Length; i++)
		{
			inventory.Add(equipment[i].equipment.Clone());
		}
	}

	public bool SellTo(Item i, Inventory customer)
	{
		if (buyBack.Contains(i)) return BuyBack(i, customer);

		if (inventory.Contains(i))
		{
			Purse p = customer.GetComponent<Purse>();
			if (customer.EmptySpace && p.Pay(i.Value))
			{
				return customer.AddItem(i);
			}
		}
		return false;
	}

	public bool BuyBack(Item i, Inventory customer)
	{
		if (buyBack.Contains(i))
		{
			Purse p = customer.GetComponent<Purse>();
			if (customer.EmptySpace && p.Pay(i.Value))
			{
				return customer.AddItem(i);
			}
		}
		return false;
	}

	public bool BuyFrom(Item i, Inventory n)
	{
		Purse p = n.GetComponent<Purse>();
		if (n.RemoveItem(i))
		{
			Add(i);
			p.AddBits(SellValue(i));
			return true;
		}
		return false;
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

	public static int SellValue(Item i)
	{
		if (i.value == 0) return 0;
		return 1 + i.value / 7;
	}
}
