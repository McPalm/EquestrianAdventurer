using System;
using System.Collections.Generic;

static public class IntVector2Utility
{
	



	/// <summary>
	/// Get everything between two points in the shape of a rectangle
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	static public IntVector2[] GetRect(IntVector2 a, IntVector2 b)
	{
		List<IntVector2> ret = new List<IntVector2>();

		for(int x = Math.Min(a.x, b.x); x < Math.Max(a.x, b.x)+1; x++)
		{
			for (int y = Math.Min(a.y, b.y); y < Math.Max(a.y, b.y) + 1; y++)
			{
				ret.Add(new IntVector2(x, y));
			}
		}

		return ret.ToArray();
	}

	static public int DeltaSum(IntVector2 a, IntVector2 b)
	{
		return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
	}

	static public int DeltaMax(IntVector2 a, IntVector2 b)
	{
		return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
	}

	static public int PFDistance(IntVector2 a, IntVector2 b)
	{
		return Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y)) + Math.Min(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y)) / 2;
	}
}
