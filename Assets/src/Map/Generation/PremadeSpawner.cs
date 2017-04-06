using UnityEngine;
using System.Collections;

public class PremadeSpawner : CreatureSpawner
{
	const string PATH = "spawn/";

	public SpawnContainer[] spawnlist;

	public override void Spawn()
	{
		foreach(SpawnContainer sc in spawnlist)
		{
			SpawnAt(sc.target, sc.offset.x, sc.offset.y);
		}
	}

	static public PremadeSpawner Get(string name)
	{
		return Resources.Load<PremadeSpawner>(PATH + name);
	}

	[System.Serializable]
	public struct SpawnContainer
	{
		public IntVector2 offset;
		public GameObject target;
	}
}
