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
		switch(s.ToLower())
		{
			case "open shop":
				ShopInventory shop = null;
				if (talkTo)
					shop = talkTo.GetComponent<ShopInventory>();
				if (shop)
					shop.OpenShop();
				yield break;
		}


	}
}
