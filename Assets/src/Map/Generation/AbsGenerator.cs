using UnityEngine;
using System.Collections;
using System;

public abstract class AbsGenerator : IGenerator
{
	protected int[][] map;

	virtual public IntVector2 ModuleAnchor
	{
		get
		{
			return new IntVector2(21, 21);
		}
	}

	public abstract void Generate(CompassDirection connections, bool module);


	/// <summary>
	/// every 0 in a generated map adds one to the player map. (its wierd)
	/// </summary>
	/// <param name=""></param>
	protected void AddResults(int[][] other)
	{
		for (int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (other[x][y] == 0) map[x][y]++;
			}
		}
	}

	/// <summary>
	/// every 0 in a generated map adds one to the player map. (its wierd)
	/// </summary>
	/// <param name=""></param>
	protected void UnionResults(int[][] other)
	{
		for (int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (other[x][y] > map[x][y]) map[x][y] = other[x][y];
			}
		}
	}

	/// <summary>
	/// transfer all 0 in the mask to the map
	/// </summary>
	/// <param name="mask"></param>
	protected void Mask(int[][] mask)
	{
		for (int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (mask[x][y] == 0) map[x][y] = 0;
			}
		}
	}

	public int[][] GetResult()
	{
		int[][] copy = new int[map.Length][];
		for (int x = 0; x < copy.Length; x++)
		{
			copy[x] = new int[map[0].Length];
			for (int y = 0; y < copy[0].Length; y++)
			{
				copy[x][y] = map[x][y];
			}
		}

		return copy;
	}
}
