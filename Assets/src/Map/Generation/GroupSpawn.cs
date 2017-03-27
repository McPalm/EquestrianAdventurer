using UnityEngine;
using System.Collections.Generic;

public class GroupSpawn : MonoBehaviour
{

	public int expansions = 5;

	public GameObject[] spawnthese;

	// Use this for initialization
	void Start ()
	{
		// get a buncha valid tiles
		List<IntVector2> locations = new List<IntVector2>();
		locations.Add(IntVector2.RoundFrom(transform.position));
		
		for(int i = 0; i < expansions; i++)
		{
			AddAdjacent((Vector2)locations[i], locations);
		}

		// spawn stuffs on these tiles
		for(int i = 0; i < spawnthese.Length; i++)
		{
			if (locations.Count == 0) break; // in case we run out of spez
			int rand = Random.Range(0, locations.Count);
			Instantiate(spawnthese[i], (Vector2)locations[rand], Quaternion.identity);
			locations.RemoveAt(rand);
		}
		Destroy(gameObject);
	}


	void AddAdjacent(Vector2 origin, List<IntVector2> list)
	{
		BlockMap b = BlockMap.Instance;
		if (b.BlockMove(origin + Vector2.up) == false && list.Contains(IntVector2.RoundFrom(origin + Vector2.up)) == false)
			list.Add(IntVector2.RoundFrom(origin + Vector2.up));
		if (b.BlockMove(origin + Vector2.right) == false && list.Contains(IntVector2.RoundFrom(origin + Vector2.right)) == false)
			list.Add(IntVector2.RoundFrom(origin + Vector2.right));
		if (b.BlockMove(origin + Vector2.down) == false && list.Contains(IntVector2.RoundFrom(origin + Vector2.down)) == false)
			list.Add(IntVector2.RoundFrom(origin + Vector2.down));
		if (b.BlockMove(origin + Vector2.left) == false && list.Contains(IntVector2.RoundFrom(origin + Vector2.left)) == false)
			list.Add(IntVector2.RoundFrom(origin + Vector2.left));
	}
}
