using UnityEngine;
using System.Collections;

public class LOSCheck : MonoBehaviour
{
	public bool HasLOS(MapObject t)
	{
		return HasLOS(GetComponent<MapObject>(), t);
	}

	static public bool HasLOS(MapObject a, MapObject b)
	{
		RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)a.RealLocation, (Vector2)b.RealLocation, 1 << 9);

		foreach (RaycastHit2D hit in hits)
			if (hit.collider.GetComponent<Wall>().BlockSight) return false;

		return true;
	}
}
