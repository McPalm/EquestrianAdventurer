using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
	public string displayName;
	public Sprite sprite;
	public int value;

	public float red = 1f;
	public float green = 1f;
	public float blue = 1f;

	public ItemCategory category;


	virtual public string Tooltip
	{
		get
		{
			return displayName + "(" + value + " bits)";
		}
	}

	public Color Tint
	{
		get
		{
			return new Color(red, green, blue, 1f);
		}
		set
		{
			red = value.r;
			green = value.g;
			blue = value.b;
		}
	}

	virtual public int Value
	{
		get
		{
			return value;
		}

		set
		{
			this.value = value;
		}
	}

	virtual public Item Clone()
	{
		Item i = new Item();

		i.displayName = displayName;
		i.sprite = sprite;
		i.value = value;

		i.red = red;
		i.green = green;
		i.blue = blue;

		i.category = category;

		return i;
	}
}
