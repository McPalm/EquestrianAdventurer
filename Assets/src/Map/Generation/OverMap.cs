using UnityEngine;
using System.Collections.Generic;

public class OverMap : MonoBehaviour {

	Dictionary<IntVector2, MapSectionContainer> map = new Dictionary<IntVector2, MapSectionContainer>();


	// Use this for initialization
	void Start ()
	{
		MapSectionContainer con = new MapSectionContainer();
		con.AddConnection(CompassDirection.east | CompassDirection.north | CompassDirection.west);
		map.Add(IntVector2.zero, con);

		// east
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.west);
		map.Add(new IntVector2(1, 0), con);

		//north
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.west | CompassDirection.south);
		map.Add(new IntVector2(0, 1), con);

		// west
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.east | CompassDirection.north);
		map.Add(new IntVector2(-1, 0), con);

		// northwest
		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.south | CompassDirection.east);
		map.Add(new IntVector2(-1, 1), con);



		foreach (IntVector2 iv2 in map.Keys)
			LoadSection(iv2);
	}
	
	void LoadSection(IntVector2 iv2)
	{
		MapSectionContainer msc;

		map.TryGetValue(iv2, out msc);

		if(msc.section == null)
		{
			msc.LoadContainer(iv2);
			
		}
	}

	[System.Serializable]
	class MapSectionContainer
	{
		public MapSection section;
		public CompassDirection connections;

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

			RoomChain5by5 generator = new RoomChain5by5();
			generator.Generate(connections);

			section.LoadFromBlueprint(generator.GetResult());
		}
	}
}
