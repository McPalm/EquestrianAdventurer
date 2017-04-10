using UnityEngine;
using System.Collections;

public class Discount : MonoBehaviour
{
	[Range(0f, 1f)]
	public float discount = 0.1f;
	public string[] flags;

	void Start()
	{
		GetComponent<ShopInventory>().EventOpenShop.AddListener(OnOpenShop);
	}

	void OnOpenShop(ShopInventory i)
	{
		float totalDiscount = 1f;
		foreach(string s in flags)
		{
			if (StoryFlags.Instance.HasFlag(s))
				totalDiscount *= 1f - discount;
		}
		i.priceMultiplier = totalDiscount;
	}
}
