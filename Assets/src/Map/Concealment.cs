using UnityEngine;
using System.Collections.Generic;

public class Concealment : MonoBehaviour
{
	static HashSet<IntVector2> locations = new HashSet<IntVector2>();

	void OnEnable()
	{
		locations.Add(IntVector2.RoundFrom(transform.position));
	}

	void OnDisable()
	{
		locations.Remove(IntVector2.RoundFrom(transform.position));
	}

	static public bool ConcealmentAt(IntVector2 v2)
	{
		return locations.Contains(v2);
	}
}
