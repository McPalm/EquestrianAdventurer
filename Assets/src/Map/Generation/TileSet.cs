using UnityEngine;
using System.Collections;

public class TileSet : MonoBehaviour
{
	static public TileSet GetTileSet(string name)
	{
		TileSet r = Resources.Load<TileSet>("tileset/" + name);
		if (r) return r;
		Debug.LogWarning("Cannot find tileset " + name + ". Using default instead!");
		return Resources.Load<TileSet>("tileset/default");
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