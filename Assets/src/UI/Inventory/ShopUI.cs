﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
	public DropArea BuyBack;
	public DropArea Stock;
	public ShopInventory model;

	UIInventory playerInventory;

	static public ShopUI Instance;

	public GameObject OnScreenAnchor;

	public UnityEvent EventOpenShopInventory;

	void Awake()
	{
		Instance = this;
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
		playerInventory = FindObjectOfType<UIInventory>();
	}

	public void Open(ShopInventory inventory)
	{
		if(model) Close();
		gameObject.SetActive(true);
		model = inventory;
		model.EventPutInBuyBack.AddListener(ModelAddBuyback);
		model.EventDestroyItem.AddListener(ModelRemoveItem);
		transform.position = OnScreenAnchor.transform.position;
		Build();
		EventOpenShopInventory.Invoke();
	}

	public void Close()
	{
		foreach (UIItem i in GetComponentsInChildren<UIItem>())
			UIItemPool.Instance.Deactivate(i.Item);

		if (model)
		{
			model.EventPutInBuyBack.RemoveListener(ModelAddBuyback);
			model.EventDestroyItem.RemoveListener(ModelRemoveItem);
		}
		model = null;
		gameObject.SetActive(false);
	}

	public void Close(ShopInventory si)
	{
		if (si == model) Close();
	}

		void Build()
	{
		foreach(Item i in model.inventory)
		{
			UIItem ui = UIItemPool.Instance.Get(i);
			ui.transform.position = Stock.anchor.transform.position;
			ui.DropIn(Stock);
		}
		foreach (Item i in model.buyBack)
		{
			UIItem ui = UIItemPool.Instance.Get(i);
			ui.transform.position = Stock.anchor.transform.position;
			ui.DropIn(BuyBack);
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
		for(int c = 0; c < playerInventory.Consumables.Length; c++)
		{
			if(destination == playerInventory.Consumables[c])
			{
				model.SellTo(i.Item, playerInventory.model, c);
			}
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

	void ModelRemoveItem(Item i)
	{
		UIItemPool.Instance.Deactivate(i);
	}
}

