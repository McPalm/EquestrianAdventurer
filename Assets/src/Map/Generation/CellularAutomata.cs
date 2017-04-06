using UnityEngine;
using System.Collections;

public class CellularAutomata : IGenerator
{
	int[][] map;

	public float density = 0.44f;
	public int iterations = 3;

	public IntVector2 ModuleAnchor
	{
		get
		{
			return new IntVector2(21, 21);
		}
	}

	public void Generate(CompassDirection connections, bool module)
	{
		map = new int[MapSectionData.DIMENSIONS][];
		for(int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			map[x] = new int[MapSectionData.DIMENSIONS];
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (x == 0 && ((connections & CompassDirection.west) == 0))
					map[x][y] = 1;
				else if (y == 0 && ((connections & CompassDirection.south) == 0))
					map[x][y] = 1;
				else if (x == MapSectionData.DIMENSIONS - 1 && ((connections & CompassDirection.east) == 0))
					map[x][y] = 1;
				else if (y == MapSectionData.DIMENSIONS - 1 && ((connections & CompassDirection.north) == 0))
					map[x][y] = 1;
				else map[x][y] = (Random.value < density) ? 1 : 0;
			}
		}


		for(int i = 0; i < iterations; i++)
		{
			smooth();
		}
	}

	public void GenerateFrom(int[][] origin, int iterations)
	{
		map = new int[MapSectionData.DIMENSIONS][];

		for (int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			map[x] = new int[MapSectionData.DIMENSIONS];
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				map[x][y] = (origin[x][y] == 0) ? 1 : 0;
			}
		}

		for (int i = 0; i < iterations; i++)
		{
			smooth();
		}
	}

	void smooth()
	{
		for (int x = 1; x < MapSectionData.DIMENSIONS - 1; x++)
		{
			for (int y = 1; y < MapSectionData.DIMENSIONS - 1; y++)
			{
				int nearwalls = 0;
				for (int lx = x - 1; lx < x + 2; lx++)
				{
					for (int ly = y - 1; ly < y + 2; ly++)
					{
						nearwalls += map[lx][ly];
					}
				}
				map[x][y] = (nearwalls > 4) ? 1 : 0;
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

	public int[][] GetInverted()
	{
		int[][] copy = new int[map.Length][];
		for (int x = 0; x < copy.Length; x++)
		{
			copy[x] = new int[map[0].Length];
			for (int y = 0; y < copy[0].Length; y++)
			{
				copy[x][y] = (map[x][y] == 1) ? 0  : 1;
			}
		}

		return copy;
	}
}
