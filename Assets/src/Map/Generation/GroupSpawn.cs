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
		
		for(int i = 0; i < expansions && i < locations.Count; i++)
		{
			AddAdjacent(locations[i], locations);
		}

		if (locations.Count < spawnthese.Length)
		{
			Destroy(gameObject);
			return;
		}
		
		// spawn stuffs on these tiles
		for (int i = 0; i < spawnthese.Length; i++)
		{ 
			int rand = Random.Range(0, locations.Count);
			Instantiate(spawnthese[i], (Vector2)locations[rand], Quaternion.identity);
			locations.RemoveAt(rand);
		}

		Destroy(gameObject);
	}


	void AddAdjacent(IntVector2 origin, List<IntVector2> list)
	{
		BlockMap b = BlockMap.Instance;
		if (b.BlockMove(origin + IntVector2.up) == false && list.Contains(IntVector2.up) == false)
			list.Add(origin + IntVector2.up);
		if (b.BlockMove(origin + IntVector2.right) == false && list.Contains(IntVector2.right) == false)
			list.Add(origin + IntVector2.right);
		if (b.BlockMove(origin + IntVector2.down) == false && list.Contains(origin + IntVector2.down) == false)
			list.Add(origin + IntVector2.down);
		if (b.BlockMove(origin + IntVector2.left) == false && list.Contains(IntVector2.left) == false)
			list.Add(origin + IntVector2.left);
	}
}
