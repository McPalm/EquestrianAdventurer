using UnityEngine;
using System.Collections.Generic;

public class CreatureSpawner : MonoBehaviour
{
	const string PATH = "spawn/";

	public GameObject[] creatures;
	public MapSection targetSection;

	public int desiredSpawnPoints = 7;
	public int minSpawnDistance = 6;

	virtual public void Spawn()
	{
		foreach (IntVector2 point in SpawnPoints(targetSection, desiredSpawnPoints, minSpawnDistance / 2, minSpawnDistance))
		{
			SpawnAt(creatures[Random.Range(0, creatures.Length)], point.x, point.y);
		}
	}

	virtual protected List<IntVector2> SpawnPoints(MapSection section, int desiredSpawnPoints = 7, int padding = 3, int spawndistance = 6)
	{
		List<IntVector2> ret = new List<IntVector2>();
		HashSet<IntVector2> used = new HashSet<IntVector2>();
		if (section.HasModule)
		{
			if (!section.modulePrefab.blockSpawn)
				ret.Add(section.ModuleLocation);
			used.Add(section.ModuleLocation);
		}

		System.Func<IntVector2, bool> ValidSpot = (where) =>
		{
			foreach(IntVector2 iv2 in used)
			{
				if ((where - iv2).MagnitudePF <= spawndistance) return false;
			}
			if (BlockMap.Instance.BlockMove(where)) return false;

			return true;
		};

		for(int i = 0; i < 10000; i++)
		{
			IntVector2 suggested = new IntVector2(Random.Range(padding, MapSectionData.DIMENSIONS - padding), Random.Range(padding, MapSectionData.DIMENSIONS - padding));
			if(ValidSpot(suggested))
			{
				ret.Add(suggested);
				used.Add(suggested);
				if (ret.Count == desiredSpawnPoints) return ret;
			}
		}
		Debug.LogWarning("Could not find enough spawnpoints at " + targetSection + " after 10000 tries");
		return ret;
	}

	

	protected void SpawnAt(GameObject o, int x, int y)
	{
		Instantiate(o, targetSection.transform.position + new Vector3(x, y), Quaternion.identity);
	}

	static public CreatureSpawner Get(string name)
	{
		print(PATH + name);
		CreatureSpawner ps = Resources.Load<CreatureSpawner>(PATH + name);
		if (!ps) Debug.LogWarning("Cannot creature spawning file for " + name);
		return ps;
	}

	static public bool HasSpawner(string name)
	{
		return Resources.Load(PATH + name) != null;
	}
}

