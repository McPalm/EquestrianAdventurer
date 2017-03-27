﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
	public string displayName;
	public Sprite sprite;
	public int value;

	virtual public string Tooltip
	{
		get
		{
			return displayName;
		}
	}
}
