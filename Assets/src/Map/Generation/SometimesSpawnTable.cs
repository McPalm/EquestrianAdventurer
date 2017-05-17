using UnityEngine;
using System.Collections;

public class SometimesSpawnTable : SpawnTable
{

	[SerializeField, Range(0f, 1f)]
	float dropChance = 1f;

	// Use this for initialization
	new void Start ()
	{
		if (Random.value < dropChance)
			base.Start();
	}
	
}
