﻿using UnityEngine;
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
			print(GetComponent<Image>());
			print(item.sprite);
			GetComponent<Image>().sprite = item.sprite;
		}
	}

	new protected void Start()
	{
		target = transform;
		base.Start();
	}

}
