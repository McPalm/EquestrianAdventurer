using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverMap : MonoBehaviour {

	Dictionary<IntVector2, MapSectionContainer> map = new Dictionary<IntVector2, MapSectionContainer>();

	public GameObject LoadingIcon;
	public MapType debugType;

	// Use this for initialization
	void Start ()
	{
		FindObjectOfType<RogueController>().GetComponent<Mobile>().EventMovement.AddListener(PlayerMoveEvent);

		MapSectionContainer con = new MapSectionContainer();
		
		con.AddConnection(CompassDirection.south);
		con.terrain = MapType.forest;
		map.Add(IntVector2.zero, con);

		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.east);
		con.terrain = MapType.forest;
		map.Add(new IntVector2(1, 2), con);

		IntVector2[] forest = new IntVector2[]
		{
			IntVector2.zero,
			new IntVector2(-1, 0),
			new IntVector2(1, 0),
			new IntVector2(-1, 1),
			new IntVector2(0, 1),
			new IntVector2(1, 1),
			new IntVector2(-1, 2),
			new IntVector2(0, 2),
			new IntVector2(1, 2)
		};

		con = new MapSectionContainer();
		con.AddConnection(CompassDirection.west);
		con.terrain = MapType.rooms;
		map.Add(new IntVector2(2, 2), con);

		IntVector2[] castle = new IntVector2[]
		{
			new IntVector2(2, 2),
			new IntVector2(2, 3),
			new IntVector2(3, 2),
			new IntVector2(3, 3),
			new IntVector2(3, 4)
		};

		InitSections(debugType, forest); // the initial biome. using debug. for testing ofc.

		InitSections(MapType.rooms, castle);

		RandomizeConnections(forest);
		RandomizeConnections(castle);

		/*
		foreach (IntVector2 iv2 in map.Keys)
			StartCoroutine(LoadSection(iv2));
			*/
		StartCoroutine(LoadOnDemand(IntVector2.zero)); // load around the player
	}


	void InitSections(MapType terrain, params IntVector2[] sections)
	{
		for(int i = 0; i < sections.Length; i++)
		{
			GetSectionAt(sections[i]).terrain = terrain;
		}
	}

	void RandomizeConnections(params IntVector2[] sections)
	{
		List<IntVector2> unconnected = new List<IntVector2>();
		List<IntVector2> connected = new List<IntVector2>();

		int entrances = 0;

		for(int i = 0; i < sections.Length; i++)
		{
			MapSectionContainer c = GetSectionAt(sections[i]);
			if (c.connections == CompassDirection.nowhere)
				unconnected.Add(sections[i]);
			else
			{
				connected.Add(sections[i]);
				entrances++;
				c.GenInfo = entrances;
			}
		}

		int attempts = 0;
		while (unconnected.Count > 0)
		{
			// step 1: pick a random connected section
			int r = Random.Range(0, connected.Count);
			// step 2: expand into random adjacent section
			IntVector2 into = RandomNearby(connected[r]);
			
			if (unconnected.Contains(into))
			{
				Bridge(into, connected[r]);
				GetSectionAt(into).GenInfo = GetSectionAt(connected[r]).GenInfo;
				connected.Add(into);
				unconnected.Remove(into);
			}
			// step 3: goto step one untill all sections are connected
			attempts++;
			if (attempts > 10000) break;
		}
		print("Build Attempts: " + attempts);


		// TODO
		// step 4: bridge together separated sections
		attempts = 0;
		int connectionsMade = 0;
		while(connectionsMade < entrances - 1)
		{
			int r = Random.Range(0, connected.Count);
			while (GetSectionAt(connected[r]).GenInfo != connectionsMade + 1)
			{
				r = Random.Range(0, connected.Count);
				attempts++;
				if (attempts > 10000) break;
			}
			if (attempts > 10000) break;

			IntVector2 into = RandomNearby(connected[r]);
			if (connected.Contains(into) && GetSectionAt(into).GenInfo == connectionsMade + 2)
			{
				Bridge(into, connected[r]);
				connectionsMade++;
			}

			attempts++;
		}

		print("Bridge Attempts: " + attempts);
		// step 5: add a random number of extra bridges
		// TODO (maybe)

		for(int i = 0; i < 1 + Mathf.Sqrt(sections.Length); i++)
		{
			int r = Random.Range(0, connected.Count);
			IntVector2 into = RandomNearby(connected[r]);
			if (connected.Contains(into))
			{
				Bridge(into, connected[r]);
			}
		}
	}


	IntVector2 lastSection = new IntVector2(-9999, -9999);

	void PlayerMoveEvent(Vector2 destination, Vector2 direction)
	{
		IntVector2 currentSection = IntVector2.FloorFrom(destination / MapSectionData.DIMENSIONS);
		if(lastSection != currentSection)
		{
			lastSection = currentSection;
			StartCoroutine(LoadOnDemand(lastSection));
		}
	}

	IEnumerator LoadOnDemand(IntVector2 centre)
	{
		LoadingIcon.SetActive(true);

		if (map.ContainsKey(centre) && GetSectionAt(centre).Loaded == false)
			yield return LoadSection(centre);

		for(int x = -1; x < 2; x++)
		{
			for (int y = -1; y < 2; y++)
			{
				if (x == 0 && y == 0) continue;
				IntVector2 next = new IntVector2(centre.x + x, centre.y + y);
				if (map.ContainsKey(next) && GetSectionAt(next).Loaded == false)
					yield return LoadSection(next);
			}
		}
		LoadingIcon.SetActive(false);
	}

	IntVector2 RandomNearby(IntVector2 v2)
	{
		switch (Random.Range(0, 4))
		{
			case 0:
				return new IntVector2(v2.x - 1, v2.y);
			case 1:
				return new IntVector2(v2.x + 1, v2.y);
			case 2:
				return new IntVector2(v2.x, v2.y - 1);
			default:
				return new IntVector2(v2.x, v2.y + 1);
		}
	}

	IEnumerator LoadSection(IntVector2 iv2)
	{
		MapSectionContainer msc;

		map.TryGetValue(iv2, out msc);

		if (msc.section == null && msc.Loaded == false)
		{
			msc.LoadContainer(iv2);
			msc.Loaded = true;

			yield return new WaitForSeconds(0f);

			CreatureSpawner cs = GetComponent<CreatureSpawner>();
			cs.targetSection = msc.section;
			cs.Spawn();
		}
	}

	MapSectionContainer GetSectionAt(IntVector2 v2)
	{
		MapSectionContainer ret = null;
		map.TryGetValue(v2, out ret);
		if (ret != null) return ret;

		ret = new MapSectionContainer();
		map.Add(v2, ret);
		return ret;
	}

	/// <summary>
	/// Get the mapsection at the given location, if there is no section at the given location, return null
	/// </summary>
	/// <param name="v2"></param>
	/// <returns></returns>
	public MapSection SectionAt(Vector2 v2)
	{
		MapSectionContainer retd = null;
		map.TryGetValue(IntVector2.FloorFrom(v2 / MapSectionData.DIMENSIONS), out retd);
		if (retd == null) return null;
		return retd.section;
	}

	void Bridge(IntVector2 a, IntVector2 b)
	{
		MapSectionContainer ac = GetSectionAt(a);
		MapSectionContainer bc = GetSectionAt(b);

		if(a.x > b.x)
		{
			ac.AddConnection(CompassDirection.west);
			bc.AddConnection(CompassDirection.east);
		}
		if (a.x < b.x)
		{
			ac.AddConnection(CompassDirection.east);
			bc.AddConnection(CompassDirection.west);
		}
		if (a.y < b.y)
		{
			ac.AddConnection(CompassDirection.north);
			bc.AddConnection(CompassDirection.south);
			
		}
		if (a.y > b.y)
		{
			ac.AddConnection(CompassDirection.south);
			bc.AddConnection(CompassDirection.north);
		}
	}

	[System.Serializable]
	class MapSectionContainer
	{
		public MapSection section;
		public CompassDirection connections;
		public MapType terrain;

		bool loaded = false;
		int genInfo = 0; // temporary data storage during generation (i think)

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

		public int GenInfo
		{
			get
			{
				return genInfo;
			}

			set
			{
				genInfo = value;
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
					palete = new int[] { 7, 5, 6, 2, 3, 4 };
					break;
				case MapType.minimumPath:
					generator = new MinimumPath();
					palete = new int[] { 0, 1 };
					break;
				case MapType.cave:
					generator = new CaveGenerator();
					palete = new int[] { 7, 8, 2 };
					break;
				default:
					generator = new RoomChain5by5();
					palete = new int[] { 1, 0 };
					break;
			}

			generator.Generate(connections);

			section.LoadFromBlueprint(generator.GetResult(), palete);
		}
	}
}
