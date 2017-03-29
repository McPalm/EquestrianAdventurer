using UnityEngine;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
	public DropArea BuyBack;
	public DropArea Stock;
	public ShopInventory model;

	UIInventory playerInventory;

	// Use this for initialization
	void Start ()
	{
		playerInventory = FindObjectOfType<UIInventory>();
		Build();

		model.EventPutInBuyBack.AddListener(ModelAddBuyback);
		
	}

	void Build()
	{
		foreach(Item i in model.inventory)
		{
			Stock.Drop(UIItemPool.Instance.Get(i), null);
		}

		Stock.EventMoveOut.AddListener(OnDragItemsOut);
		BuyBack.EventMoveOut.AddListener(OnDragItemsOut);
		Stock.EventDropHere.AddListener(OnDropInShop);
		BuyBack.EventDropHere.AddListener(OnDropInShop);
	}

	void OnDragItemsOut(Dropable d, DropArea source, DropArea destination)
	{
		UIItem i = d.GetComponent<UIItem>();
		if (!i) return;
		if (destination == playerInventory.Equipment)
		{
			model.SellTo(i.Item, playerInventory.model);
		}
	}

	void OnDropInShop(Dropable d, DropArea source, DropArea destination)
	{
		UIItem i = d.GetComponent<UIItem>();
		if (!i) return;
		if (source == playerInventory.Equipment)
		{
			model.BuyFrom(i.Item, playerInventory.model);
		}
	}

	void ModelAddBuyback(Item i)
	{
		UIItem ui = UIItemPool.Instance.Get(i);
		ui.DropIn(BuyBack);
		
	}
}

