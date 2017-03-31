﻿using UnityEngine;
using System.Collections;


/// <summary>
/// Simply create a game object with this item in it.
/// Add this component
/// Put item in the item
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MapObject))]
public class GroundItem : MonoBehaviour
{
	public Item item;
	
	protected void Start()
	{
		item = CloneItem();
		GetComponent<SpriteRenderer>().sprite = item.sprite;
		GetComponent<SpriteRenderer>().color = item.Tint;
		GetComponent<SpriteRenderer>().sortingOrder = 10;

		GetComponent<MapObject>().Put(transform.position);
	}

	virtual public Item CloneItem()
	{
		if (item.sprite == null)
		{
			item.sprite = GetComponent<SpriteRenderer>().sprite;
			item.Tint = GetComponent<SpriteRenderer>().color;
		}
		return item.Clone();
	}
}
