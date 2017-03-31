using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ShopInventory : MonoBehaviour
{
	[HideInInspector]
	public List<Item> inventory = new List<Item>();
	[HideInInspector]
	public List<Item> buyBack = new List<Item>();

	public GroundItem[] items;

	public ItemEvent EventPutInInventory = new ItemEvent();
	public ItemEvent EventPutInBuyBack = new ItemEvent();


	// Use this for initialization
	void Awake()
	{
		GenerateInventory();
	}

	public void GenerateInventory()
	{
		inventory.Clear();
		buyBack.Clear();

		for (int i = 0; i < items.Length; i++)
		{
			inventory.Add(items[i].CloneItem());
		}
	}

	public void OpenShop()
	{
		ShopUI.Instance.Open(this);
	}

	public void CloseShop()
	{
		ShopUI.Instance.Close(this);
	}

	public bool SellTo(Item i, Inventory customer)
	{
		if (buyBack.Contains(i)) return SellBack(i, customer);

		if (inventory.Contains(i))
		{
			Purse p = customer.GetComponent<Purse>();
			if (customer.CanAccept(i) && p.Pay(i.Value))
			{
				inventory.Remove(i);
				return customer.AddItem(i);
			}
		}
		return false;
	}

	public bool SellTo(Item i, Inventory customer, int slot)
	{
		if (buyBack.Contains(i)) return SellBack(i, customer);

		if (inventory.Contains(i))
		{
			Purse p = customer.GetComponent<Purse>();
			if (customer.CanAccept(i) && p.Pay(i.Value))
			{
				inventory.Remove(i);
				return customer.AddItem(i, slot);
			}
		}
		return false;
	}

	public bool SellBack(Item i, Inventory customer)
	{
		if (buyBack.Contains(i))
		{
			Purse p = customer.GetComponent<Purse>();
			if (customer.CanAccept(i) && p.Pay(SellValue(i)))
			{
				buyBack.Remove(i);
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
		EventPutInBuyBack.Invoke(i);
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

	public class ItemEvent : UnityEvent<Item> { }
}
