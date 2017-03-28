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
}
