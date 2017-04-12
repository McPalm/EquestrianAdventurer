using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class OverMap : MonoBehaviour {

	Dictionary<IntVector2, MapSectionContainer> map = new Dictionary<IntVector2, MapSectionContainer>();

	public GameObject LoadingIcon;
	public MapType debugType;

	public CreatureSpawner forest;
	public CreatureSpawner cave;
	public CreatureSpawner castle;

	public MapSectionEvent EventEnterNewSection = new MapSectionEvent();


	public MapModule[] forestModules;
	public MapModule[] caveModules;
	public MapModule[] castleModules;
	public MapModule zecorasHut;

	[Space(10)]
	public bool testDisableSpawn;
	public MapModule testModule;


	// Use this for initialization
	void Start ()
	{
		FindObjectOfType<RogueController>().GetComponent<Mobile>().EventMovement.AddListener(PlayerMoveEvent);

		MapSectionContainer con = GetSectionAt(new IntVector2(0, -1));
		con.terrain = MapType.pregenerated;
		con.sectionName = "ponyville";

		con = GetSectionAt(new IntVector2(-4, 3));
		con.terrain = MapType.pregenerated;
		con.sectionName = "ursalair";

		con = GetSectionAt(new IntVector2(3, 4));
		con.terrain = MapType.pregenerated;
		con.sectionName = "castleboss";

		GenerateOverMap();

		/*
		foreach (IntVector2 iv2 in map.Keys)
			StartCoroutine(LoadSection(iv2));
			*/
		StartCoroutine(LoadOnDemand(new IntVector2(0, -1))); // load around the player
	}

	/// <summary>
	/// Randomize the connections between sections
	/// </summary>
	void GenerateOverMap()
	{
		MapSectionContainer con;

		// define sections

		IntVector2[] forest = IntVector2Utility.GetRect(new IntVector2(-1, 0), new IntVector2(1, 2));
		IntVector2[] castle = IntVector2Utility.GetRect(new IntVector2(2, 2), new IntVector2(4, 3));
		List<IntVector2> build = new List<IntVector2>();
		build.AddRange(IntVector2Utility.GetRect(new IntVector2(-4, 1), new IntVector2(-2, 2)));
		build.Add(new IntVector2(-3, 3));
		build.Add(new IntVector2(-2, 3));
		IntVector2[] cave = build.ToArray();

		// clear up connections in case we already had one

		foreach (IntVector2 iv2 in forest)
			GetSectionAt(iv2).connections = CompassDirection.nowhere;
		foreach (IntVector2 iv2 in castle)
			GetSectionAt(iv2).connections = CompassDirection.nowhere;
		foreach (IntVector2 iv2 in cave)
			GetSectionAt(iv2).connections = CompassDirection.nowhere;

		// setup connections between zones

		con = GetSectionAt(new IntVector2(0, 0));
		con.AddConnection(CompassDirection.south);
		con.terrain = MapType.forest;

		con = GetSectionAt(new IntVector2(1, 2));
		con.AddConnection(CompassDirection.east);
		con.terrain = MapType.forest;

		con = GetSectionAt(new IntVector2(-1, 2));
		con.AddConnection(CompassDirection.west);
		con.terrain = MapType.forest;

		con = GetSectionAt(new IntVector2(2, 2));
		con.AddConnection(CompassDirection.west);
		con.terrain = MapType.rooms;
		
		con = GetSectionAt(new IntVector2(-2, 2));
		con.AddConnection(CompassDirection.east);
		con.terrain = MapType.cave;

		con = GetSectionAt(new IntVector2(-4, 2));
		con.AddConnection(CompassDirection.north);
		con.terrain = MapType.cave;

		con = GetSectionAt(new IntVector2(3, 3));
		con.AddConnection(CompassDirection.north);
		con.terrain = MapType.cave;

		// setup terrain

		InitSections(debugType, new MapModule[] {forestModules[Random.Range(0, forestModules.Length)]}, forest); // the initial biome. using debug. for testing ofc.
		InitSections(MapType.rooms, new MapModule[] { castleModules[Random.Range(0, castleModules.Length / 2)], castleModules[Random.Range(castleModules.Length / 2, castleModules.Length)] }, castle);
		InitSections(MapType.cave, new MapModule[] { caveModules[Random.Range(0, caveModules.Length/2)], caveModules[Random.Range(caveModules.Length / 2, caveModules.Length)] }, cave);

		GetSectionAt(new IntVector2(-1, 1)).givenModule = zecorasHut;

		// roll up connections within section

		RandomizeConnections(forest);
		RandomizeConnections(castle);
		RandomizeConnections(cave);
	}

	void InitSections(MapType terrain, MapModule[] modules, params IntVector2[] sections)
	{
		for(int i = 0; i < sections.Length; i++)
		{
			GetSectionAt(sections[i]).terrain = terrain;
			if (testModule)
			{
				GetSectionAt(sections[i]).givenModule = testModule;
			}
			else GetSectionAt(sections[i]).givenModule = null;
			
		}
		for(int i = 0; i < modules.Length; i++)
		{
			int random = Random.Range(0, sections.Length);
			for(int j = 0; j < sections.Length; j++)
			{
				MapSectionContainer c = GetSectionAt(sections[(random + j) % sections.Length]);
				if (c.givenModule == null)
				{
					c.givenModule = modules[i];
					break;
				}
			}
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

		int r; // = Random.Range(0, connected.Count);
		int attempts = 0;
		while (unconnected.Count > 0)
		{
			// step 1: pick a random connected section
			r = Random.Range(0, connected.Count);
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


		// TODO
		// step 4: bridge together separated sections
		attempts = 0;
		int connectionsMade = 0;
		bool[] cb = new bool[entrances]; // keeps track of a section is connected to any of the conected sections.
		for (int i = 0; i < cb.Length; i++)
			cb[i] = false;
		cb[0] = true;
		r = Random.Range(0, connected.Count); // start at a random place
		while (connectionsMade < entrances - 1)
		{
			// check if any of the connections from R connects an connected and an unconnected section.
			// if it does, connect them, mark them both as connected
			// continue till we got wanted number of connections

			foreach (IntVector2 into in AllNearbyRandomFirst(connected[r]))
			{
				if (connected.Contains(into) && cb[GetSectionAt(connected[r]).GenInfo-1] != cb[GetSectionAt(into).GenInfo-1])
				{
					Bridge(into, connected[r]);
					connectionsMade++;
					cb[GetSectionAt(connected[r]).GenInfo-1] = true;
					cb[GetSectionAt(into).GenInfo-1] = true;
					r = Random.Range(0, connected.Count); // start at a new location
					break;
				}
			}

			r++; // = Random.Range(0, connected.Count);
			r %= connected.Count;

			attempts++;
			if (attempts > 10000) break;
		}
		if (attempts > 10000)
		{
			Debug.LogWarning("Overmap failed to make all nececcary connetions. Made: " + connectionsMade +", wanted " + (entrances - 1));
			foreach(IntVector2 c in connected)
			{
				print(c.ToString() + " " + GetSectionAt(c).GenInfo);
			}
		}

		// step 5: add a random number of extra bridges
		// TODO (maybe)

		for (int i = 0; i < 1 + Mathf.Sqrt(sections.Length); i++)
		{
			r = Random.Range(0, connected.Count);
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
			EventEnterNewSection.Invoke(GetSectionAt(lastSection).section);
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
				return v2 + IntVector2.up;
			case 1:
				return v2 + IntVector2.right;
			case 2:
				return v2 + IntVector2.down;
			default:
				return v2 + IntVector2.left;
		}
	}

	IntVector2[] AllNearbyRandomFirst(IntVector2 v2)
	{
		IntVector2[] ret = new IntVector2[4];
		switch (Random.Range(0, 4))
		{
			case 0:
				ret[0] = v2 + IntVector2.up;
				ret[1] = v2 + IntVector2.right;
				ret[2] = v2 + IntVector2.down;
				ret[3] = v2 + IntVector2.left;
				break;
			case 1:
				ret[1] = v2 + IntVector2.up;
				ret[2] = v2 + IntVector2.right;
				ret[3] = v2 + IntVector2.down;
				ret[0] = v2 + IntVector2.left;
				break;
			case 2:
				ret[2] = v2 + IntVector2.up;
				ret[3] = v2 + IntVector2.right;
				ret[1] = v2 + IntVector2.down;
				ret[0] = v2 + IntVector2.left;
				break;
			default:
				ret[3] = v2 + IntVector2.up;
				ret[0] = v2 + IntVector2.right;
				ret[1] = v2 + IntVector2.down;
				ret[2] = v2 + IntVector2.left;
				break;
		}
		return ret;
	}

	IEnumerator LoadSection(IntVector2 iv2)
	{
		MapSectionContainer msc;

		map.TryGetValue(iv2, out msc);

		if (msc.Loaded == false)
		{
			msc.LoadContainer(iv2);
			msc.Loaded = true;

			

			yield return new WaitForSeconds(0f);
			CreatureSpawner cs = null;
			if (msc.terrain == MapType.forest) cs = forest;
			if (msc.terrain == MapType.cave) cs = cave;
			if (msc.terrain == MapType.rooms) cs = castle;
			if (msc.terrain == MapType.pregenerated) cs = PremadeSpawner.Get(msc.sectionName);
			if (cs &! testDisableSpawn)
			{
				cs.targetSection = msc.section;
				cs.Spawn();
			}
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
	/// Reset and regenerate all sections except ponyville
	/// </summary>
	public void Reset()
	{
		StartCoroutine(UnloadLoad());
	}

	IEnumerator UnloadLoad()
	{
		IntVector2[] nature = IntVector2Utility.GetRect(new IntVector2(-4, 0), new IntVector2(4, 5));
		yield return  AsyncUnload(nature);
		GenerateOverMap();
		yield return new WaitForSeconds(0f);
		yield return LoadOnDemand(new IntVector2(0, -1)); // load around ponyville
	}

	public IEnumerator AsyncUnload(params IntVector2[] v2s)
	{
		LoadingIcon.SetActive(true);
		foreach(IntVector2 v2 in v2s)
		{
			Unload(v2);
			yield return new WaitForSeconds(0f);
		}
		LoadingIcon.SetActive(true);
	}

	/// <summary>
	/// Unload all sections in an array
	/// </summary>
	/// <param name="v2s"></param>
	public void Unload(params IntVector2[] v2s)
	{
		MapSectionContainer c = null;
		foreach (IntVector2 v2 in v2s)
		{
			if(map.TryGetValue(v2, out c))
			{
				if (c.Loaded)
				{
					c.section.Unload();
					c.Loaded = false;
				}
			}
		}
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
		public MapModule givenModule;
		public string sectionName; // used for pregenerated maps

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
			if (section == null)
			{
				GameObject o = new GameObject("Map Section " + iv2.ToString());
				o.transform.position = (Vector2)iv2 * MapSectionData.DIMENSIONS;
				section = o.AddComponent<MapSection>();
				section.loadSection = false;
			}
			section.modulePrefab = givenModule;
			IGenerator generator;
			int[] palette;
			switch(terrain)
			{
				case MapType.forest:
					generator = new ForestGenerator();
					palette = new int[] { 7, 5, 6, 2, 3, 4 };
					section.paletteName = "default";
					section.overlayTint = new Color(0.45f, 0.5f, 0.55f);
					break;
				case MapType.minimumPath:
					generator = new MinimumPath();
					palette = new int[] { 0, 1 };
					section.paletteName = "default";
					break;
				case MapType.cave:
					generator = new CaveGenerator();
					section.paletteName = "cave";
					palette = new int[] { 0, 1, 2, 3 };
					section.overlayTint = new Color(0.31f, 0.44f, 0.5f);
					break;
				case MapType.pregenerated:
					generator = null;
					section.loadSection = false;
					palette = null;
					section.LoadFromFilename(sectionName);
					//section.sectionName = sectionName;
					// section.overlayTint = new Color(0.6f, 0.58f, 0.5f);
					break;
				default:
					generator = new RoomChain5by5();
					palette = new int[] { 1, 0, 9, 10, 5};
					section.paletteName = "default";
					break;
			}

			if (generator != null)
			{
				generator.Generate(connections, givenModule != null);
				section.LoadFromBlueprint(generator.GetResult(), generator.ModuleAnchor, palette);
			}
		}
	}

	[System.Serializable]
	public class MapSectionEvent : UnityEvent<MapSection> { }
}
