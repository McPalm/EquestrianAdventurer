using UnityEngine;
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
		GetComponent<SpriteRenderer>().sprite = item.sprite;
		GetComponent<SpriteRenderer>().color = item.Tint;
		GetComponent<SpriteRenderer>().sortingOrder = 10; 
	}
}
