using UnityEngine;
using System.Collections;

public class TileDB : MonoBehaviour
{
	

	static public TileDB LoadPalette(string name)
	{
		TileDB r = Resources.Load<TileDB>("palette/" + name);
		if (r) return r;
		Debug.LogWarning("Cannot find palette " + name + ". Using default instead!");
		return Resources.Load<TileDB>("palette/default");
	}

	public GameObject[] tiles;

	public GameObject GetPrefab(int i)
	{
		return tiles[i];
	}

	/// <summary>
	/// Does not represent the actual tile!
	/// </summary>
	/// <param name="i"></param>
	/// <returns></returns>
	public Sprite GetSprite(int i, out Color c)
	{
		c = tiles[i].GetComponent<SpriteRenderer>().color;
		return tiles[i].GetComponent<SpriteRenderer>().sprite;
	}
}
