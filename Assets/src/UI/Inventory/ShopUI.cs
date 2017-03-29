using UnityEngine;
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
		transform.position = OnScreenAnchor.transform.position;
		Build();
		EventOpenShopInventory.Invoke();
	}

	public void Close()
	{

		if (model)
			model.EventPutInBuyBack.RemoveListener(ModelAddBuyback);
		model = null;
		gameObject.SetActive(false);
	}

	void Build()
	{
		foreach(Item i in model.inventory)
		{
			UIItemPool.Instance.Get(i).DropIn(Stock);
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

