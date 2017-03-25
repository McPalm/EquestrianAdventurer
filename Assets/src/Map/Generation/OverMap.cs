using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverMap : MonoBehaviour {

	Dictionary<IntVector2, MapSectionContainer> map = new Dictionary<IntVector2, MapSectionContainer>();


	// Use this for initialization
	void Start ()
	{
		MapSectionContainer con = new MapSectionContainer();
		con.AddConnection(CompassDirection.east | CompassDirection.north | CompassDirection.west);
		con.terrain = MapType.forest;
		map.Add(IntVector2.zero, con);

		// east
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.west);
		con.terrain = MapType.forest;
		map.Add(new IntVector2(1, 0), con);

		//north
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.west | CompassDirection.south);
		con.terrain = MapType.forest;
		map.Add(new IntVector2(0, 1), con);

		// west
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.east | CompassDirection.north);
		con.terrain = MapType.forest;
		map.Add(new IntVector2(-1, 0), con);

		// northwest
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.south | CompassDirection.east);
		con.terrain = MapType.forest;
		map.Add(new IntVector2(-1, 1), con);



		foreach (IntVector2 iv2 in map.Keys)
			StartCoroutine(LoadSection(iv2));
	}


	IEnumerator LoadSection(IntVector2 iv2)
	{
		MapSectionContainer msc;

		map.TryGetValue(iv2, out msc);

		if(msc.section == null)
		{
			msc.LoadContainer(iv2);
		}
		yield return new WaitForSeconds(0f);

		CreatureSpawner cs = GetComponent<CreatureSpawner>();
		cs.targetSection = msc.section;
		cs.Spawn();
	}

	[System.Serializable]
	class MapSectionContainer
	{
		public MapSection section;
		public CompassDirection connections;
		public MapType terrain;

		bool loaded = false;

		public bool Loaded
		{
			get
			{
				return loaded;
			}

			set
			{
				loaded = value;
			}
		}

		public void AddConnection(CompassDirection d)
		{
			connections |= d;
		}

		public void SeverConnection(CompassDirection d)
		{
			connections ^= d;
		}

		public void LoadContainer(IntVector2 iv2)
		{
			GameObject o = new GameObject("Map Section " + iv2.ToString());
			o.transform.position = (Vector2)iv2 * MapSectionData.DIMENSIONS;
			section = o.AddComponent<MapSection>();

			IGenerator generator;
			int[] palete;
			switch(terrain)
			{
				case MapType.forest:
					generator = new ForestGenerator();
					palete = new int[] { 0, 5, 6, 2 };
					break;
				default:
					generator = new RoomChain5by5();
					palete = new int[] { 0, 1 };
					break;
			}

			generator.Generate(connections);

			section.LoadFromBlueprint(generator.GetResult(), palete);
		}
	}
}
