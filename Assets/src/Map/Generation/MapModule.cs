using UnityEngine;
using System.Collections;

public class MapModule : MonoBehaviour
{
	[HideInInspector]
	public IntVector2[] usedTiles;

	public bool blockSpawn;

	void Awake()
	{
		TileVisbility[] v = GetComponentsInChildren<TileVisbility>();
		usedTiles = new IntVector2[v.Length];
		for(int i = 0; i < v.Length; i++)
		{
			usedTiles[i] = IntVector2.RoundFrom(v[i].transform.localPosition);
		}
	}

	const string PATH = "Modules/";

	static public MapModule Get(string source)
	{
		return Resources.Load<MapModule>(PATH + source);
	}
}
