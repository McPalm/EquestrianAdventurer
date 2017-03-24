using UnityEngine;
using System.Collections;

public class CellularAutomata
{
	int[][] map;

	public float density = 0.44f;
	public int iterations = 3;

	public void Generate(int dx, int dy)
	{
		map = new int[dx][];
		for(int x = 0; x < dx; x++)
		{
			map[x] = new int[dy];
			for (int y = 0; y < dy; y++)
			{
				if (x == 0 || y == 0 || x == dx - 1 || y == dy - 1) map[x][y] = 1;
				else map[x][y] = (Random.value < density) ? 1 : 0;
			}
		}


		for(int i = 0; i < iterations; i++)
		{
			for (int x = 1; x < dx-1; x++)
			{
				for (int y = 1; y < dy-1; y++)
				{
					int nearwalls = 0;
					for(int lx = x-1; lx < x+2; lx++)
					{
						for (int ly = y-1; ly < y+2; ly++)
						{
							nearwalls += map[lx][ly];
						}
					}
					map[x][y] = (nearwalls > 4) ? 1 : 0;
				}
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
