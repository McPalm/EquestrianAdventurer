using UnityEngine;

[System.Serializable]
public struct SerializedColor
{
	public float r;
	public float g;
	public float b;
	public float a;

	public SerializedColor(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public static implicit operator Color(SerializedColor c)
	{
		return new Color(c.r, c.g, c.b, c.a);
	}

	public static implicit operator SerializedColor(Color c)
	{
		return new SerializedColor(c.r, c.g, c.b, c.a);
	}

}
