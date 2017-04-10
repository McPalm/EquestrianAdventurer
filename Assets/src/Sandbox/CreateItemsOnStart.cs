using UnityEngine;
using System.Collections;

public class CreateItemsOnStart : MonoBehaviour
{

	public string[] items;

	// Use this for initialization
	void Start ()
	{
		for(int i = 0; i < items.Length; i++)
		{
			Item item;
			if (CreateItem.Instance.TryGet(items[i], out item))
			{
				FindObjectOfType<RogueController>().GetComponent<Inventory>().AddOrPutOnGround(item);
				print("Adding " + item.displayName + " to inventory");
			}
			else
				print("missing " + items[i]);
		}
	}
}
