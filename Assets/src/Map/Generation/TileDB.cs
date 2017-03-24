using UnityEngine;
using System.Collections;

public class TileDB : MonoBehaviour
{
	static TileDB _instance;

	static public TileDB Instance
	{
		get
		{
			_instance = Resources.Load<TileDB>("TileDB");
			return _instance;
		}
	}

	public GameObject[] tiles;

	public GameObject GetPrefab(int i)
	{
		return tiles[i];
	}
}
