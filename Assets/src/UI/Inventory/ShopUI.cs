using UnityEngine;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
	public DropArea BuyBack;
	public DropArea Stock;
	public ShopInventory inventory;

	public UIItem UIItemPrefab;
	List<UIItem> active = new List<UIItem>();

	Purse playerPurse; // HACK

	// Use this for initialization
	void Start ()
	{
		playerPurse = FindObjectOfType<Purse>();
		// BuyBack.EventAdd.AddListener(Sell);
		Build();
	}

	void Build()
	{
		foreach(Item i in inventory.inventory)
		{
			Stock.Drop(Build(i));
		}
	}

	UIItem Build(Item i)
	{
		print(i.displayName);
		UIItem ret = Instantiate(UIItemPrefab);
		active.Add(ret);
		ret.transform.SetParent(transform);
		ret.Item = i;
		ret.transform.position = transform.position; // make the item over the player. Should add a cool effect of it going from the ground to the inventory
													 // ret.EventDropOutside.AddListener(ViewDropEvent);
		// ret.AllowDrop = Buy;
		return ret;
	}
}

