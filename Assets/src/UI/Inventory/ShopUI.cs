using UnityEngine;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
	public DropArea BuyBack;
	public DropArea Stock;
	public ShopInventory inventory;

	// Use this for initialization
	void Start ()
	{
		Build();
	}

	void Build()
	{
		foreach(Item i in inventory.inventory)
		{
			print(i);
			print(Stock);
			Stock.Drop(UIItemPool.Instance.Get(i), null);
		}
	}
}

