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
	public UnityEvent EventStartConversation;

	void Awake()
	{
		Instance = this;
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start ()
	{
		playerInventory = FindObjectOfType<UIInventory>();

		playerInventory.EventSellItem.AddListener(OnPlayerInventorySellItem);

		Stock.EventMoveOut.AddListener(OnDragItemsOut);
		BuyBack.EventMoveOut.AddListener(OnDragItemsOut);
		Stock.EventDropHere.AddListener(OnDropInShop);
		BuyBack.EventDropHere.AddListener(OnDropInShop);
		Stock.EventClick.AddListener(OnShiftClickItemInShop);
		BuyBack.EventClick.AddListener(OnShiftClickItemInShop);
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
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if (source == playerInventory.Equipment)
		{
			model.BuyFrom(ui.Item, playerInventory.model);
		}
		for(int i = 0; i < playerInventory.Consumables.Length; i++)
		{
			if(source == playerInventory.Consumables[i])
			{
				model.BuyFrom(ui.Item, playerInventory.model);
			}
		}
	}

	void OnPlayerInventorySellItem(Item i)
	{
		if (!gameObject.activeSelf) return;

		model.BuyFrom(i, playerInventory.model);
	}

	void OnShiftClickItemInShop(Dropable d)
	{
		UIItem ui = d.GetComponent<UIItem>();
		if (!ui) return;
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			model.SellTo(ui.Item, playerInventory.model);
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

	public void Talk()
	{
		DialogueStarter d = model.GetComponent<DialogueStarter>();
		if (d)
		{
			d.StartConversation();
			EventStartConversation.Invoke();
			Close();
		}
	}
}

