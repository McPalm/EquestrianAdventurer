using UnityEngine;
using System.Collections;

/// <summary>
/// Script that spawns a random item from a set of tables
/// Rare table, pick a random item from it 2% of the times
/// Uncommon table, pick a random item from it 15% of the times
/// Common table, picks a random item fmo here when none of the others was picked
/// </summary>

public class SpawnTable : MonoBehaviour
{

	public GameObject[] Common;
	public GameObject[] Uncommon;
	public GameObject[] Rare;

	// Use this for initialization
	protected void Start ()
	{
		GameObject Spawn;
		if(Rare.Length > 0 && Random.value < 0.02f)
		{
			Spawn = Rare[Random.Range(0, Rare.Length)];
		}
		else if (Uncommon.Length > 0 && Random.value < 0.15f)
		{
			Spawn = Uncommon[Random.Range(0, Uncommon.Length)];
		}
		else
		{
			Spawn = Common[Random.Range(0, Common.Length)];
		}

		Instantiate(Spawn, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

}
