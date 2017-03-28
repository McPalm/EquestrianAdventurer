using UnityEngine;
using System.Collections;

public class ShopUI : MonoBehaviour
{
	public DropArea BuyBack;

	// Use this for initialization
	void Start ()
	{
		BuyBack.EventAdd.AddListener(Sell);
	}
	
	void Sell(Dropable d)
	{
		UIItem i = d.GetComponent<UIItem>();

		if(i)
		{
			print("Sold " + i.Item.displayName + ". Its worth " + i.Item.value);
		}
	}
}
