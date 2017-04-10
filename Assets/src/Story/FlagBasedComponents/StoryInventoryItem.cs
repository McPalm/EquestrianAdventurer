using UnityEngine;
using System.Collections;

public class StoryInventoryItem : MonoBehaviour
{
	[SerializeField]
	string requiredflag;
	[SerializeField]
	GroundItem[] items;

	// Use this for initialization
	void Start ()
	{
		GetComponent<ShopInventory>().EventGenerateInventory.AddListener(OnGenerateInventory);
	}
	
	// Update is called once per frame
	void OnGenerateInventory(ShopInventory i)
	{
		if (StoryFlags.Instance.HasFlag(requiredflag))
		{
			foreach(GroundItem g in items)
				i.AddToStock(g.CloneItem());
		}
	}
}
