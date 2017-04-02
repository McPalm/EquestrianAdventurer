using UnityEngine;
using System.Collections.Generic;

public class MapSection : MonoBehaviour
{
	
	public string sectionName;
	public string paletteName;
	public Color overlayTint = Color.gray;

	public bool loadSection;

	Dictionary<IntVector2, GameObject> map = new Dictionary<IntVector2, GameObject>();
	MapSectionData data;
	TileDB tileDB;
	

	void Start()
	{
		if (data == null)
		{
			print("banana!");
			if (loadSection)
			{
				data = MapSectionData.Load(sectionName);
				print(data);
				if (data == null) data = new MapSectionData(sectionName);
				print(data);
			}
			else
				data = new MapSectionData(sectionName);
			DrawAll();
		}
	}

	/// <summary>
	/// Removes all tiles, 
	/// unloads this section,
	/// remove everyone and everything on it
	/// </summary>
	public void Unload()
	{
		print("removing " + name);
		foreach(GameObject o in map.Values)
		{
			Destroy(o);
		}
		map.Clear();

		// remove all map objects within this sections confines...  somehow
		foreach (MapObject o in ObjectMap.Instance.GetRange((int)transform.position.x, (int)transform.position.y, (int)transform.position.x + MapSectionData.DIMENSIONS, (int)transform.position.y + MapSectionData.DIMENSIONS))
		{
			Destroy(o.gameObject);
		}
	}

	public void DrawAll()
	{
		tileDB = TileDB.LoadPalette(paletteName);

		for(int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for(int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				if (data.tiles[x][y] >= 0) DrawSprite(new Vector2(x, y), data.tiles[x][y]);
			}
		}
	}


	public void SetTile(Vector3 location, int t)
	{
		// save file edit
		location = transform.InverseTransformVector(location);
		IntVector2 v2 = IntVector2.RoundFrom(location);
		data.tiles[v2.x][v2.y] = t;
		// physical edit
		DrawSprite(location, t);
	}

	public void Fill(Vector3 location, int t)
	{
		location = transform.InverseTransformVector(location);
		IntVector2 origin = IntVector2.RoundFrom(location);

		print(data);
		print(data.tiles);
		print(data.tiles[origin.x][origin.y]);

		int filter = data.tiles[origin.x][origin.y];

		List<Vector2> locations = new List<Vector2>();
		locations.Add(location);
		for(int i = 0; i < locations.Count && i < MapSectionData.DIMENSIONS * MapSectionData.DIMENSIONS; i++)
		{
			int x = (int)locations[i].x;
			int y = (int)locations[i].y;
			if (x > 0	&! locations.Contains(new Vector2(x - 1, y))	&& data.tiles[x - 1][y] == filter) locations.Add(new Vector2(x - 1, y));
			if (x < MapSectionData.DIMENSIONS - 1 & ! locations.Contains(new Vector2(x + 1, y))	&& data.tiles[x + 1][y] == filter) locations.Add(new Vector2(x + 1, y));
			if (y > 0	&! locations.Contains(new Vector2(x, y - 1))	&& data.tiles[x][y - 1] == filter) locations.Add(new Vector2(x, y - 1));
			if (y < MapSectionData.DIMENSIONS-1 & ! locations.Contains(new Vector2(x, y + 1))	&& data.tiles[x][y + 1] == filter) locations.Add(new Vector2(x, y + 1));
		}

		foreach(Vector2 v2 in locations)
		{
			DrawSprite(v2, t);
			data.tiles[(int)v2.x][(int)v2.y] = t;
		}
	}

	// true if inside map, false if the location isoutside the bounds of the map section.
	public bool IsInSection(Vector2 location)
	{
		if (location.y < transform.position.y - 0.5f || location.y > transform.position.y + MapSectionData.DIMENSIONS - 0.5f) return false;
		if (location.x < transform.position.x - 0.5f || location.x > transform.position.x + MapSectionData.DIMENSIONS - 0.5f) return false;
		return true;
	}

	/// <summary>
	/// Draw a sprite at wherever
	/// </summary>
	/// <param name="v2">Local position</param>
	/// <param name="i">Tile</param>
	void DrawSprite(Vector2 v2, int t)
	{
		EraseAt(v2);
		GameObject tile = Instantiate(tileDB.GetPrefab(t), transform.position + (Vector3)v2, Quaternion.identity) as GameObject;
		tile.transform.SetParent(transform);
		map.Add(IntVector2.RoundFrom(v2), tile);

		/*
		TileAt(v2).sprite = TileDB.GetTile(t, out flip, out wall);
		TileAt(v2).flipX = flip;
		TileAt(v2).sortingLayerName = (wall) ? "Active" : "Map";
		*/
	}


	public void EraseAt(Vector2 location)
	{
		GameObject deleteme;
		if (map.TryGetValue(IntVector2.RoundFrom(location), out deleteme))
		{
			Destroy(deleteme);
			map.Remove(IntVector2.RoundFrom(location));
		}
	}

	/// <summary>
	/// Tile at Local Position
	/// </summary>
	/// <param name="v2"></param>
	/// <returns></returns>
	GameObject TileAt(Vector2 v2)
	{
		GameObject ret = null;
		IntVector2 iv2 = IntVector2.RoundFrom(v2);
		if (map.TryGetValue(iv2, out ret))
		{
			return ret;
		}

		GameObject o = new GameObject(sectionName + " " + IntVector2.RoundFrom(v2));
		SpriteRenderer s = o.AddComponent<SpriteRenderer>();
		o.transform.SetParent(transform);
		o.transform.localPosition = v2;
		s.sortingOrder = 1 - Mathf.RoundToInt(ret.transform.position.y * 10);
		map.Add(iv2, ret);

		return ret;
	}

	public void Save()
	{
		data.Save();
	}

	public void LoadFromBlueprint(int[][] map, params int[] palette)
	{
		if(data == null)
		{
			data = new MapSectionData(name);
		}
		for(int x = 0; x < MapSectionData.DIMENSIONS; x++)
		{
			for(int y = 0; y < MapSectionData.DIMENSIONS; y++)
			{
				data.tiles[x][y] = palette[map[x][y]%palette.Length];
			}
		}
		DrawAll();
	}

	static public MapSection BuildSection(Vector2 offset, string name)
	{
		GameObject o = new GameObject(name + "(section)");
		o.transform.position = offset;
		MapSection s = o.AddComponent<MapSection>();
		s.sectionName = name;
		return s;
	}
}
