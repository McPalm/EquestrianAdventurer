using UnityEngine;
using System.Collections;
using System;

public class ForestGenerator : IGenerator
{
	int[][] map;

	public void Generate(CompassDirection connections)
	{
		map = new int[MapSectionData.DIMENSIONS][];
		for(int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			map[x] = new int[MapSectionData.DIMENSIONS];
		}
		
		CellularAutomata generator = new CellularAutomata();
		generator.Generate(0);
		AddResults(generator.GetResult());
		generator.Generate(0);
		AddResults(generator.GetResult());

	}

	/// <summary>
	/// every 0 in a generated map adds one to the player map. (its wierd)
	/// </summary>
	/// <param name=""></param>
	void AddResults(int[][] other)
	{
		for (int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (other[x][y] == 0) map[x][y]++;
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
