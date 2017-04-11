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



	/// <summary>
	/// Randomly place a single type of tile on anotehr type of tile
	/// </summary>
	/// <param name="placethis">the tile we want to place</param>
	/// <param name="placeon">what we wish to place it on</param>
	/// <param name="density">the chance for it to be placed per tile tested</param>
	/// <param name="buffer">the minimum area around the tile that has to bee of the same tile.</param>
	protected void ScatterSingle(int placethis, int placeon, float density, int buffer = 1)
	{
		for (int x = buffer; x < MapSectionData.DIMENSIONS - buffer; x++)
		{
			for (int y = buffer; y < MapSectionData.DIMENSIONS - buffer; y++)
			{
				if (UnityEngine.Random.value < density)
				{
					bool clear = true;

					for (int lx = x - buffer; lx < x + 1 + buffer; lx++)
					{
						for (int ly = y - buffer; ly < y + 1 + buffer; ly++)
						{
							if (map[lx][ly] != placeon) clear = false;
						}
					}

					if (clear)
						map[x][y] = placethis;
				}
			}
		}
	}

	/// <summary>
	/// Randomly place a single type of tile, but forbit it to be placed on another specific type of tile
	/// </summary>
	/// <param name="placethis">the tile to place</param>
	/// <param name="forbid">the tile we cannot place it on</param>
	/// <param name="density">the chance per valid tile that we place it on it</param>
	/// <param name="buffer">the minimmum area around the tile that cannot contain the forbidden tile</param>
	protected void ScatterSingleForbid(int placethis, int forbid, float density, int buffer = 1)
	{
		for (int x = buffer; x < MapSectionData.DIMENSIONS - buffer; x++)
		{
			for (int y = buffer; y < MapSectionData.DIMENSIONS - buffer; y++)
			{
				if (UnityEngine.Random.value < density)
				{
					bool clear = true;

					for (int lx = x - buffer; lx < x + 1 + buffer; lx++)
					{
						for (int ly = y - buffer; ly < y + 1 + buffer; ly++)
						{
							if (map[lx][ly] == forbid) clear = false;
						}
					}

					if (clear)
						map[x][y] = placethis;
				}
			}
		}
	}

	/// <summary>
	/// Randomly place a single type of tile, but forbit it to be placed on another specific type of tile. Then populate the buffer with anotehr type of tile
	/// </summary>
	/// <param name="placethis">the tile to place</param>
	/// <param name="forbid">the tile we cannot place it on</param>
	/// <param name="density">the chance per valid tile that we place it on it</param>
	/// <param name="buffer">the minimmum area around the tile that cannot contain the forbidden tile</param>
	/// <param name="sorroundWith">the type of tile we wish to sorround the placed tile with</param>
	/// <param name="chance">the chance that a tile in the buffer zone gets converted</param>
	protected void ScatterSingleForbidSorround(int placethis, int forbid, float density, int buffer, int sorroundWith, float chance)
	{
		for (int x = buffer; x < MapSectionData.DIMENSIONS - buffer; x++)
		{
			for (int y = buffer; y < MapSectionData.DIMENSIONS - buffer; y++)
			{
				if (UnityEngine.Random.value < density)
				{
					bool clear = true;

					for (int lx = x - buffer; lx < x + 1 + buffer; lx++)
					{
						for (int ly = y - buffer; ly < y + 1 + buffer; ly++)
						{
							if (map[lx][ly] == forbid) clear = false;
						}
					}

					if (clear)
					{
						map[x][y] = placethis;
						for (int lx = x - buffer; lx < x + 1 + buffer; lx++)
						{
							for (int ly = y - buffer; ly < y + 1 + buffer; ly++)
							{
								if (lx == x && ly == y) continue;
								if (UnityEngine.Random.value < chance)
									map[lx][ly] = sorroundWith;
							}
						}
					}
				}
			}
		}
	}
}
