using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MapObject))]
public class SightRadius : MonoBehaviour
{
	MapObject mapObject;

	public int sightRadius = 9;

	Dictionary<IntVector2, TileVisbility> allTiles = new Dictionary<IntVector2, TileVisbility>();

	static SightRadius _instance;
	public static SightRadius Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<SightRadius>();
			return _instance;
		}
	}

	// Use this for initialization
	void Start ()
	{
		mapObject = GetComponent<MapObject>();
		StartCoroutine(FirstFrame());
	}

	IEnumerator FirstFrame()
	{
		yield return new WaitForSeconds(0f);
		RefreshView();
	}

	public bool CanSee(TileVisbility v)
	{
		return true;
		// throw new System.NotImplementedException();
	}

	public void RefreshView()
	{
		for(int x = mapObject.RealLocation.x - sightRadius; x < mapObject.RealLocation.x +  sightRadius; x++)
		{
			for (int y = mapObject.RealLocation.y - sightRadius; y < mapObject.RealLocation.y + sightRadius; y++)
			{
				TileVisbility v = null;
				if(allTiles.TryGetValue(new IntVector2(x, y), out v))
				{
					if (!v.Visible && CanSee(v))
						v.Show();
				}
			}
		}
	}

	public void AddVisbility(TileVisbility v, IntVector2 location)
	{
		allTiles.Add(location, v);
	}

	public void RemoveTile(IntVector2 location)
	{
		allTiles.Remove(location);
	}
}
