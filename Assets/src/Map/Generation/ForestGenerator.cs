using UnityEngine;
using System.Collections;
using System;

public class ForestGenerator : AbsGenerator
{
	public float foilageDensity = 0.2f;

	public override void Generate(CompassDirection connections)
	{
		map = new int[MapSectionData.DIMENSIONS][];
		for(int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			map[x] = new int[MapSectionData.DIMENSIONS];
		}
		
		// Stack three cellular Autoamata generations to create a natural looking type of terrain
		CellularAutomata generator = new CellularAutomata();
		generator.density = 0.55f;
		generator.iterations = 2;
		generator.Generate(connections);
		AddResults(generator.GetResult());
		generator.Generate(0);
		AddResults(generator.GetResult());
		generator.density = 0.45f;
		generator.iterations = 5;
		generator.Generate(0);
		AddResults(generator.GetResult());

		// Union with a minimum path o make sure we got connection to other areas
		MinimumPath minPath = new MinimumPath();
		minPath.Generate(connections);
		UnionResults(minPath.GetResult());

		// scatter a few walls around that will be turned into trees and rocks in the next step
		// they will only be placed in such spots that they do not block any potential paths.
		for (int x = 1; x < MapSectionData.DIMENSIONS - 1; x++)
		{
			for (int y = 1; y < MapSectionData.DIMENSIONS - 1; y++)
			{
				if (UnityEngine.Random.value < foilageDensity)
				{
					if (map[x][y] == 1 || map[x][y] == 2) // generate on grass
					{
						int nearbywalls = 0;
						// cardinals
						if (map[x - 1][y] == 0) nearbywalls++;
						if (map[x + 1][y] == 0) nearbywalls++;
						if (map[x][y - 1] == 0) nearbywalls++;
						if (map[x][y + 1] == 0) nearbywalls++;
						// diagonals
						if (map[x - 1][y - 1] == 0) nearbywalls++;
						if (map[x - 1][y + 1] == 0) nearbywalls++;
						if (map[x + 1][y - 1] == 0) nearbywalls++;
						if (y < MapSectionData.DIMENSIONS - 1 && map[x + 1][y + 1] == 0) nearbywalls++;

						if (nearbywalls == 0)
							map[x][y] = 0;
					}
				}
			}
		}

		// find isolated walls and turn into rocks or trees
		for (int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (map[x][y] == 0)
				{
					int nearbywalls = 0;
					if (x > 0 && map[x - 1][y] == 0) nearbywalls++;
					if (x < MapSectionData.DIMENSIONS - 1 && map[x + 1][y] == 0) nearbywalls++;
					if (y > 0 && map[x][y - 1] == 0) nearbywalls++;
					if (y < MapSectionData.DIMENSIONS - 1 && map[x][y + 1] == 0) nearbywalls++;
					if (nearbywalls == 0)
						map[x][y] = (UnityEngine.Random.value < 0.15f) ? 4 : 5;
				}
			}
		}
	}
}
