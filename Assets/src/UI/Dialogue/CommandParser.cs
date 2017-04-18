using UnityEngine;
using System.Collections;

/// <summary>
/// A class that parses and runs command given by Yarn Scripts
/// </summary>

public class CommandParser : MonoBehaviour
{
	static CommandParser _instance;

	public GameObject talkTo;
	public GameObject player;

	public static CommandParser Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<CommandParser>();
			return _instance;
		}
	}

	public IEnumerator RunCommand(string s)
	{
		if(s.ToLower() == "open shop")
		{ 
			ShopInventory shop = null;
			if (talkTo)
				shop = talkTo.GetComponent<ShopInventory>();
			if (shop)
				shop.OpenShop();
			yield break;
		}
		string string4 = s.Substring(0, 4).ToLower();
		switch(string4)
		{
			case "give":
				string itemName = s.Substring(5);
				Item item = null;
				if (CreateItem.Instance.TryGet(itemName.ToLower(), out item))
					player.GetComponent<Inventory>().AddOrPutOnGround(item);
				else
					Debug.LogError("Unable to find " + itemName + " in Item Database");
				// string item 
				yield break;
			case "take":
				if (!player.GetComponent<Inventory>().RemoveItem(s.Substring(5)))
					Debug.LogError("Failed to remove item " + s.Substring(5));
				yield break;

		}


		yield break;
	}
}
