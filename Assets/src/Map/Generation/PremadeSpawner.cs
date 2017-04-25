using UnityEngine;
using System.Collections;

public class PremadeSpawner : CreatureSpawner
{
	

	public SpawnContainer[] spawnlist;

	public override void Spawn()
	{
		foreach(SpawnContainer sc in spawnlist)
		{
			SpawnAt(sc.target, sc.offset.x, sc.offset.y);
		}
	}

	

	[System.Serializable]
	public struct SpawnContainer
	{
		public IntVector2 offset;
		public GameObject target;
	}
}
