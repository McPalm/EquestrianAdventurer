using System;
using UnityEngine;

[System.Serializable]
#pragma warning disable
public struct IntVector2 : IEquatable<IntVector2>
#pragma warning restore
{
	public int x;
	public int y;

	public IntVector2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public bool Equals(IntVector2 other)
	{
		return other.x == x && other.y == y;
	}

	public static IntVector2 RoundFrom(Vector2 v2)
	{
		return new IntVector2(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y));
	}

	public static IntVector2 FloorFrom(Vector2 v2)
	{
		return new IntVector2(Mathf.FloorToInt(v2.x), Mathf.FloorToInt(v2.y));
	}

	static public IntVector2 zero
	{
		get
		{
			return new IntVector2(0, 0);
		}
	}

	static public IntVector2 MaxValue
	{
		get
		{
			return new IntVector2(int.MaxValue, int.MaxValue);
		}
	}

	static public IntVector2 up
	{
		get
		{
			return new IntVector2(0, 1);
		}
	}

	static public IntVector2 right
	{
		get
		{
			return new IntVector2(1, 0);
		}
	}

	static public IntVector2 left
	{
		get
		{
			return new IntVector2(-1, 0);
		}
	}

	static public IntVector2 down
	{
		get
		{
			return new IntVector2(0, -1);
		}
	}

	static public IntVector2 operator *(IntVector2 a, int b)
	{
		return new IntVector2(a.x * b, a.y * b);
	}

	static public IntVector2 operator +(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x + b.x, a.y + b.y);
	}

	static public IntVector2 operator -(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x - b.x, a.y - b.y);
	}

	public static explicit operator Vector2(IntVector2 a)
	{
		return new Vector2(a.x, a.y);
	}

	public static explicit operator Vector3(IntVector2 a)
	{
		return new Vector3(a.x, a.y);
	}

	public static bool operator == (IntVector2 a, IntVector2 b)
	{
		return a.x == b.x && a.y == b.y;
	}

	public static bool operator != (IntVector2 a, IntVector2 b)
	{
		return a.x != b.x || a.y != b.y;
	}

	public override string ToString()
	{
		return string.Format("[X = {0}; Y = {1};]", x, y);
	}

	public int DeltaSum(IntVector2 other)
	{
		return Math.Abs(x - other.x) + Math.Abs(y - other.y);
	}

	public int DeltaMax(IntVector2 other)
	{
		return Math.Max(Math.Abs(x - other.x), Math.Abs(y - other.y));
	}

	public int MagnitudeMax
	{
		get
		{
			return Math.Max(Math.Abs(x), Math.Abs(y));
		}
	}

	public int MagnitudeSum
	{
		get
		{
			return Math.Abs(x) + Math.Abs(y);
		}
	}

	public int MagnitudePF
	{
		get
		{
			if(x*x > y*y)
				return Math.Abs(x) + Math.Abs(y)/2;
			else
				return Math.Abs(x)/2 + Math.Abs(y);
		}
	}
}
