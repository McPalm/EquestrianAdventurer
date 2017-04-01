using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIItem : Dropable
{
	Item item;

	public Item Item
	{
		get
		{
			return item;
		}

		set
		{
			item = value;
			sortValue = item.Value;
			if (item is Valuable) sortValue *= 7f;
			GetComponent<Tooltip>().hint = item.Tooltip;
			GetComponent<Image>().sprite = item.sprite;
			GetComponent<Image>().color = item.Tint;
		}
	}

	new protected void Start()
	{
		target = transform;
		base.Start();
	}


}
