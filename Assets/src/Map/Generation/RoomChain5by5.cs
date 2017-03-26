using UnityEngine;
using System.Collections;

public class RoomChain5by5 : IGenerator
{
	int[][] map;

	public int tileDimension = 41;
	public int roomSize = 8; // including wall
	public int roomCountDimension = 5;


	public void Generate(CompassDirection connections)
	{
		map = new int[tileDimension][];
		for(int x = 0; x < tileDimension; x++)
		{
			map[x] = new int[tileDimension];
			for (int y = 0; y < tileDimension; y++)
				map[x][y] = 1; // wall is default
		}


		// step one, rooms. keepin track of nearby rooms
		// 0 = no room
		// 1 = main room
		// 2 = jank room
		// 0 = no door
		// 1 = doorright
		// 2 = doorup
		// 3 = doorboth
		int[][] roomgrid = new int[roomCountDimension][];
		int[][] doorgrid = new int[roomCountDimension][];
		for (int x = 0; x < roomCountDimension; x++)
		{
			roomgrid[x] = new int[roomCountDimension];
			doorgrid[x] = new int[roomCountDimension];
		}
			

		

		float r = Random.value;
		if(r < 0.37f)
		{
			//plus
			for (int i = 0; i < roomCountDimension; i++)
			{
				roomgrid[roomCountDimension / 2][i] = 1;
				roomgrid[i][roomCountDimension / 2] = 1;
			}
		}
		else if(r < 0.71f)
		{
			// diamond
			for (int i = 0; i < roomCountDimension; i++)
			{
				roomgrid[roomCountDimension / 2][i] = 1;
				roomgrid[i][roomCountDimension / 2] = 1;
			}
			roomgrid[roomCountDimension / 2][roomCountDimension / 2] = 0;
			roomgrid[roomCountDimension / 2+1][roomCountDimension / 2+1] = 1;
			roomgrid[roomCountDimension / 2+1][roomCountDimension / 2-1] = 1;
			roomgrid[roomCountDimension / 2-1][roomCountDimension / 2+1] = 1;
			roomgrid[roomCountDimension / 2-1][roomCountDimension / 2-1] = 1;

		}
		else
		{
			for (int i = 0; i < roomCountDimension; i++)
			{
				roomgrid[0][i] = 1;
				roomgrid[i][0] = 1;
				roomgrid[roomCountDimension - 1][i] = 1;
				roomgrid[i][roomCountDimension - 1] = 1;

			}
		}


		// connect doors on main path // step two, connections, between rooms.
		for (int x = 0; x < roomCountDimension; x++)
		{
			for(int y = 0; y < roomCountDimension; y++)
			{
				if(roomgrid[x][y] == 1)
				{
					if (x < roomCountDimension-1 && roomgrid[x + 1][y] == 1) doorgrid[x][y] = 1;
					if (y < roomCountDimension-1 && roomgrid[x][y + 1] == 1) doorgrid[x][y] += 2;
				}
			}
		}
		
		
		// generate random extra rooms
		for (int i = 0; i < roomCountDimension - 2; i++) // passes based on size
		{
			for (int x = 0; x < roomCountDimension; x++)
			{
				for (int y = 0; y < roomCountDimension; y++)
				{
					if (roomgrid[x][y] == 0 && Random.value < 0.25f)
					{
						roomgrid[x][y] = 2;
						// connect to nearby rooms
						bool alone = true;
						if (x > 0 && roomgrid[x-1][y] != 0 && Random.value < 0.7f)
						{
							if (doorgrid[x-1][y] < 2)
							{
								doorgrid[x-1][y] += 1;
								alone = false;
							}
						}
						if (x < 4 && roomgrid[x+1][y] != 0 && Random.value < 0.7f)
						{
							if (doorgrid[x][y] == 0 || doorgrid[x][y] == 2)
							{
								doorgrid[x][y] += 1;
								alone = false;
							}
						}
						if (y < 4 && roomgrid[x][y+1] != 0 && Random.value < 0.7f)
						{
							if (doorgrid[x][y] == 0 || doorgrid[x][y] == 2)
							{
								doorgrid[x][y] += 2;
								alone = false;
							}
						}
						if (y > 0 && roomgrid[x][y-1] != 0 && Random.value < 0.7f)
						{
							if (doorgrid[x][y-1] < 2)
							{
								doorgrid[x][y-1] += 2;
								alone = false;
							}
						}
						if (alone)
							roomgrid[x][y] = 0;
					}
				}
			}
		}
		

		// actually build map
		for (int rx = 0; rx < roomCountDimension; rx++)
		{
			for (int ry = 0; ry < roomCountDimension; ry++)
			{
				if(roomgrid[rx][ry] != 0)
				{
					BuildRoomAt(1+rx*roomSize, 1+ry* roomSize, roomSize-1, roomSize-1);
				}
				if(doorgrid[rx][ry] > 1)
				{
					// door up
					if (Random.value < 0.2f)
					{
						// tear down wall
						BuildRoomAt(rx * roomSize + 1, ry * roomSize + roomSize - 1, roomSize - 1, 3, 1);
					}
					else if (Random.value < 0.1f)
					{
						// two doors
						map[rx * roomSize + 2][ry * roomSize + roomSize] = 2;
						map[rx * roomSize + roomSize - 2][ry * roomSize + roomSize] = 2;
					}
					else if (Random.value < 0.3f)
					{
						// random door placement
						map[rx * roomSize + Random.Range(2, roomSize-1)][ry * roomSize + roomSize] = 2;
					}
					else
						// door in the middle
						map[rx * roomSize + roomSize/2][ry * roomSize + roomSize] = 2;

				}
				if (doorgrid[rx][ry] == 1 || doorgrid[rx][ry] == 3)
				{
					// door right
					if (Random.value < 0.2f)
					{
						// tear down wall
						BuildRoomAt(rx * roomSize + roomSize - 1, ry * roomSize + 1, 3, roomSize - 1, 1);
					}
					else if (Random.value < 0.1f)
					{
						// two doors
						map[rx * roomSize + roomSize][ry * roomSize + 2] = 2;
						map[rx * roomSize + roomSize][ry * roomSize + roomSize - 2] = 2;
					}
					else if (Random.value < 0.3f)
					{
						map[rx * roomSize + roomSize][ry * roomSize + Random.Range(2, roomSize - 1)] = 2;
					}
					else
						map[rx * roomSize + roomSize][ry * roomSize + roomSize/2] = 2;
				}
			}
		}
		// make external connections
		if( (connections & CompassDirection.east) != 0)
		{
			map[tileDimension - 1][tileDimension / 2] = 0;
		}
		if ((connections & CompassDirection.north) != 0)
		{
			map[tileDimension / 2][tileDimension - 1] = 0;
		}
		if ((connections & CompassDirection.west) != 0)
		{
			map[0][tileDimension / 2] = 0;
		}
		if ((connections & CompassDirection.south) != 0)
		{
			map[tileDimension / 2][0] = 0;
		}

		// step four, spawn points
	}

	void BuildRoomAt(int cornerx, int cornery, int width, int height, int pattern = 0)
	{
		if (pattern == 0) pattern = System.Math.Min(Random.Range(1, 4), Random.Range(1, 4));
		// pattern 1 : regular
		// pattern 2 : rounded corners
		// pattern 3 : around the edges

		for (int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				if (pattern == 3 && x > 1 && x < width - 2 && y > 1 && y < height - 2) continue; // dont fill center on pattern 2
				if (pattern == 2 && ((x == 0 && y == 0) || (x == width - 1 && y == 0) || (x == 0 && y == height - 1) || (x == width - 1 && y == height - 1))) continue;
				map[x+cornerx][y+cornery] = 0;
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
