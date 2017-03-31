using UnityEngine;
using System.Collections;

/// <summary>
/// this is a component for AIs
/// But the static is for line of sight.
/// Its...  uh.. poorly thought out I admit.
/// </summary>
public class LOSCheck : MonoBehaviour
{
	public int sightRadius = 8;
	MapObject me;

	void Start()
	{
		me = GetComponent<MapObject>();
	}

	public bool HasLOS(MapObject t)
	{
		if (IntVector2Utility.PFDistance(me.RealLocation, t.RealLocation) <= sightRadius)
			return HasLOS(me, t);
		return false;
	}

	static public bool HasLOS(MapObject a, MapObject b)
	{
		RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)a.RealLocation, (Vector2)b.RealLocation, 1 << 9);

		foreach (RaycastHit2D hit in hits)
			if (hit.collider.GetComponent<Wall>().BlockSight) return false;

		return true;
	}
}
