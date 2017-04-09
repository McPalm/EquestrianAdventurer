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

	/// <summary>
	/// A special LOS that takes cover and concealment into account
	/// If there is cover in between, sightradius is effectively halved. (rounded down)
	/// </summary>
	/// <param name="t"></param>
	/// <param name="cover"></param>
	/// <returns></returns>
	public bool HasLOS(MapObject t, bool cover, bool concealment)
	{
		if (!cover) return HasLOS(t);
		int distance = IntVector2Utility.PFDistance(me.RealLocation, t.RealLocation);
		if (distance > sightRadius) return false;
		if(HasLOS(me, t))
		{

			if (InCover(me, t))
				if (concealment && t.HasConcealment)
					return distance <= sightRadius / 4;
				else
					return distance <= sightRadius / 2;
			else if (concealment && t.HasConcealment)
				return distance <= sightRadius / 2;
			else
				return true;
		}
		return false;
	}

	public bool HasLOE(MapObject t, int range = 0)
	{
		if (range == 0) range = sightRadius;
		if (IntVector2Utility.PFDistance(me.RealLocation, t.RealLocation) <= range)
			return HasLOE(me, t);
		return false;
	}

	public bool HasLOE(IntVector2 t, int range = 0)
	{
		
		if (range == 0) range = sightRadius;
		if (IntVector2Utility.PFDistance(me.RealLocation, t) <= range)
		{
			return HasLOS(me.RealLocation, t);
		}
		return false;
	}

	static public bool HasLOS(MapObject a, MapObject b)
	{
		RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)a.RealLocation, (Vector2)b.RealLocation, 1 << 9);

		foreach (RaycastHit2D hit in hits)
			if (hit.collider.GetComponent<IMapBlock>().BlockSight) return false;

		return true;
	}

	static public bool HasLOS(IntVector2 a, IntVector2 b)
	{
		RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)a, (Vector2)b, 1 << 9);

		foreach (RaycastHit2D hit in hits)
			if (hit.collider.GetComponent<IMapBlock>().BlockSight) return false;

		return true;
	}

	static public bool InCover(MapObject a, MapObject b)
	{
		RaycastHit2D hits = Physics2D.Linecast((Vector2)a.RealLocation, (Vector2)b.RealLocation, 1 << 10);

		return hits;
	}

	/// <summary>
	/// Line of Effect
	/// based on BlockMove instead of Sight
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	static public bool HasLOE(MapObject a, MapObject b)
	{
		RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)a.RealLocation, (Vector2)b.RealLocation, 1 << 9);

		foreach (RaycastHit2D hit in hits)
			if (hit.collider.GetComponent<IMapBlock>().BlockMove) return false;

		return true;
	}
}
