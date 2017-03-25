﻿using UnityEngine;
using System.Collections;

public class CreatureSpawner : MonoBehaviour
{

	public GameObject[] creatures;
	public MapSection targetSection;

	public int openTilesPerCreature;

	public void Spawn()
	{
		int nextSpawn = Random.Range(0, openTilesPerCreature);
		for(int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for (int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (BlockMap.Instance.BlockMove(new Vector2(x, y) +  (Vector2)targetSection.transform.position)) continue;
				nextSpawn--;
				if (nextSpawn < 0)
				{
					SpawnAt(x, y);
					nextSpawn = Random.Range(0, openTilesPerCreature) + Random.Range(0, openTilesPerCreature);
				}
			}
		}
	}

	void SpawnAt(int x, int y)
	{
		Instantiate(creatures[Random.Range(0, creatures.Length)], targetSection.transform.position + new Vector3(x, y), Quaternion.identity);
	}
}