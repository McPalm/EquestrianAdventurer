﻿using UnityEngine;
using System.Collections.Generic;

public class ObjectMap : MonoBehaviour
{
	static ObjectMap _instance;

	public static ObjectMap Instance
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType<ObjectMap>();
			return _instance;
		}
	}

	Dictionary<IntVector2, ObjectStack> map = new Dictionary<IntVector2, ObjectStack>();

	public void Add(MapObject o)
	{
		GetStack(o.RealLocation).Add(o);
	}

	public void Move(MapObject o, IntVector2 from, IntVector2 to)
	{
		GetStack(from).Remove(o);
		GetStack(to).Add(o);
	}

	public void Remove(MapObject o)
	{
		//print(o);
		GetStack(o.RealLocation).Remove(o);
	}

	ObjectStack GetStack(IntVector2 v2)
	{
		ObjectStack o = null;
		if (map.TryGetValue(v2, out o))
			return o;
		o = new ObjectStack();
		map.Add(v2, o);
		return o;
	}

	public MapObject[] ObjectsAtLocation(IntVector2 v2)
	{
		return GetStack(v2).objects.ToArray();
	}

	/// <summary>
	/// Get the last MapCharacter on the stack at a given location
	/// Null if there is no character there
	/// </summary>
	/// <param name="v2"></param>
	/// <returns></returns>
	public MapCharacter CharacterAt(IntVector2 v2)
	{
		foreach(MapObject o in GetStack(v2).objects)
		{
			if (o.MapCharacter) return o.MapCharacter;
		}
		return null;
	}

	class ObjectStack
	{
		public List<MapObject> objects = new List<MapObject>(4);

		public int Count {  get { return objects.Count; } }

		public void Add(MapObject o)
		{
			objects.Add(o);
		}

		public void Remove(MapObject o)
		{
			//print(o);
			//print(objects.Count);
			objects.Remove(o);
		}
	}

	/// <summary>
	/// Get every mapobjects within specified rectangle
	/// </summary>
	/// <param name="minX">minx X, inclusive</param>
	/// <param name="minY">min Y, inclusive</param>
	/// <param name="maxX">max X, exlusive</param>
	/// <param name="maxY">max Y, exlusve</param>
	/// <returns></returns>
	public MapObject[] GetRange(int minX, int minY, int maxX, int maxY)
	{
		List<MapObject> list = new List<MapObject>();
		ObjectStack os = null;
		for (int x = minX; x < maxX; x++)
		{
			for (int y = minY; y < maxY; y++)
			{
				if (map.TryGetValue(new IntVector2(x, y), out os))
				{
					foreach (MapObject o in os.objects)
					{
						list.Add(o);
					}
				}
			}
		}
		return (list.ToArray());
	}
}
