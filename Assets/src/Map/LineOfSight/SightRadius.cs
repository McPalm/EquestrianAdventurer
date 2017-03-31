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
		

		int dx = System.Math.Abs((int)v.transform.position.x - mapObject.RealLocation.x);
		int dy = System.Math.Abs((int)v.transform.position.y - mapObject.RealLocation.y);

		if(System.Math.Max(dx, dy) + System.Math.Min(dx, dy) / 2 < sightRadius)
		{
			RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)mapObject.RealLocation, v.transform.position, 1 << 9); // doing hits in walls

			int maxHits = (v.BlockSight) ? 1 : 0;
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider.GetComponent<Wall>().BlockSight) maxHits--;
				if (maxHits < 0) return false;
			}
			return true;
		}

		return false;
	}

	public void RefreshView(int buffer = 1)
	{
		for (int x = mapObject.RealLocation.x - sightRadius - buffer; x < mapObject.RealLocation.x +  sightRadius + buffer; x++)
		{
			for (int y = mapObject.RealLocation.y - sightRadius - buffer; y < mapObject.RealLocation.y + sightRadius + buffer; y++)
			{
				TileVisbility v = null;
				if(allTiles.TryGetValue(new IntVector2(x, y), out v))
				{
					if (CanSee(v))
						v.Show();
					else
						v.Hide();
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

	public bool LocationVisible(IntVector2 v2)
	{
		TileVisbility v = null;
		if(allTiles.TryGetValue(v2, out v))
		{
			return v.Visible;
		}
		return false;
	}
}
