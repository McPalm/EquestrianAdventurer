using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastleSpawner : CreatureSpawner
{
	public override void Spawn()
	{
		List<IntVector2> spawnLocations = new List<IntVector2>();

		for(int x = 0; x < 5; x++)
		{
			for(int y = 0; y < 5; y++)
			{
				IntVector2 testLocation = new IntVector2(x * 8 + 4, y * 8 + 5);
				if(!BlockMap.Instance.BlockMove(testLocation))
				{
					spawnLocations.Add(testLocation);
				}
			}
		}

		for (int i = 0; i < spawnLocations.Count; i++) // spawn stuffs into half of the rooms. rounded down...  tho to be fair it might be a good idea to do 100% spawn chance, but have like non combat encounters a plenty. Spawn other stuffs than just eneimes and loot
		{
			int r = Random.Range(0, spawnLocations.Count);
			SpawnAt(creatures[Random.Range(0, creatures.Length)], spawnLocations[r].x, spawnLocations[r].y);
			spawnLocations.RemoveAt(r);
		}
	}
}
