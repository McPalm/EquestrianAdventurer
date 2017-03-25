using UnityEngine;
using System.Collections.Generic;


public class MinimumPath : IGenerator
{

	int[][] map;

	public void Generate(CompassDirection connections)
	{
		map = new int[MapSectionData.DIMENSIONS][];
		for (int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			map[x] = new int[MapSectionData.DIMENSIONS];
		}

		// connect all external connections
		if (connections != 0)
		{
			List<IntVector2> connectionTiles = new List<IntVector2>();
			
			if ((connections & CompassDirection.east) != 0)
			{
				connectionTiles.Add(new IntVector2(MapSectionData.DIMENSIONS - 1, MapSectionData.DIMENSIONS / 2));
			}
			if ((connections & CompassDirection.north) != 0)
			{
				connectionTiles.Add(new IntVector2(MapSectionData.DIMENSIONS / 2, MapSectionData.DIMENSIONS - 1));
			}
			if ((connections & CompassDirection.west) != 0)
			{
				connectionTiles.Add(new IntVector2(0, MapSectionData.DIMENSIONS / 2));
			}
			if ((connections & CompassDirection.south) != 0)
			{
				connectionTiles.Add(new IntVector2(MapSectionData.DIMENSIONS / 2, 0));
			}

			if (connectionTiles.Count == 1)
				connectionTiles.Add(new IntVector2(MapSectionData.DIMENSIONS / 2, MapSectionData.DIMENSIONS / 2));

			// TODO randomize order

			// do the path
			for(int i = 0; i < connectionTiles.Count-1; i++)
			{
				ImprintPath(connectionTiles[i], connectionTiles[i + 1]);
			}
			
		}

	}

	public void ImprintPath(IntVector2 start, IntVector2 end)
	{
		IntVector2 current = start;

		map[current.x][current.y] = 1;

		while(current != end)
		{
			// getDelta
			int dx = end.x - current.x;
			int dy = end.y - current.y;

			float xChance = Mathf.Abs(dx) * (Random.value + 0.1f);
			float yChance = Mathf.Abs(dy) * (Random.value + 0.1f);
			if (xChance > yChance) // move closer on yaxis
				current = new IntVector2(current.x + System.Math.Sign(dx), current.y);
			else
				current = new IntVector2(current.x, current.y + System.Math.Sign(dy));

			map[current.x][current.y] = 1;
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
