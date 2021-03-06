using UnityEngine;
using UnityEngine.Events;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class OvermapData
{
	[XmlIgnore]
	public Dictionary<IntVector2, SectionContainer> sections = new Dictionary<IntVector2, SectionContainer>();
	public List<SectionGroupData> groups = new List<SectionGroupData>();

	public SectionEvent EventEditSection = new SectionEvent();
	public GroupEvent EventEditGroup = new GroupEvent();
	public SectionEvent EventRemoveSection = new SectionEvent();
	

	public SectionContainer AddSection(IntVector2 location)
	{
		SectionContainer ret = new SectionContainer();
		sections.Add(location, ret);
		ret.color = Color.HSVToRGB(Random.value, Random.Range(0.7f, 1f), Random.Range(0.5f, 1f));
		EventEditSection.Invoke(location, ret);
		return ret;
	}

	public void RemoveAt(IntVector2 location)
	{
		if(sections.ContainsKey(location))
		{
			RemoveSectionFromGroup(location);
			SectionContainer container = null;
			sections.TryGetValue(location, out container);
			sections.Remove(location);
			EventRemoveSection.Invoke(location, container);
		}
	}

	public void SetSectionGenerator(IntVector2 iv2, MapType type)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.generator = type;

			EventEditSection.Invoke(iv2, sc);
		}
	}

	public void SetSectionPregeneratedName(IntVector2 iv2, string name)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.pregeneratedName = name;

			EventEditSection.Invoke(iv2, sc);
		}
	}

	public void SetSectionGeneratorInherit(IntVector2 iv2, bool inherit)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.inheritGenerator = inherit;

			SectionGroupData group = GroupOf(iv2);
			if (inherit && group != null)
				sc.generator = group.generator;

			EventEditSection.Invoke(iv2, sc);
		}
	}

	public void SetSectionSpawntable(IntVector2 iv2, string table)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.spawntable = table;
			EventEditSection.Invoke(iv2, sc);
		}
	}

	public void SetSectionSpawnInherit(IntVector2 iv2, bool inherit)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.inheritSpawnTable = inherit;

			SectionGroupData group = GroupOf(iv2);
			if (inherit && group != null)
				sc.spawntable = group.spawntable;

			EventEditSection.Invoke(iv2, sc);
		}
	}


	public void SetSectionColor(IntVector2 iv2, Color c)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.color = c;
			EventEditSection.Invoke(iv2, sc);
		}
	}

	public void AddConnection(IntVector2 iv2, CompassDirection direction)
	{
		SectionContainer sc = null;
		if(sections.TryGetValue(iv2, out sc))
		{
			sc.connections |= direction;
			EventEditSection.Invoke(iv2, sc);
		}
	}

	public void RemoveConnection(IntVector2 iv2, CompassDirection direction)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			if ((sc.connections & direction) != 0)
			{
				sc.connections -= direction;
				EventEditSection.Invoke(iv2, sc);
			}
		}
	}

	public void AddGroup(params IntVector2[] members)
	{
		if (members.Length == 0) return;

		for(int i = 0; i < members.Length; i++)
		{
			RemoveSectionFromGroup(members[i]);

		}

		SectionGroupData group = new SectionGroupData(members);
		groups.Add(group);
		Color c = Color.HSVToRGB(Random.value, 0.8f, 0.8f);

		// float h, s, v;
		SectionContainer con = null;
		if(sections.TryGetValue(members[0], out con))
		{
			c = con.color;
			group.generator = con.generator;
			group.spawntable = con.spawntable;
		}

		group.color = c;
		EventEditGroup.Invoke(group);
		for (int i = 0; i < members.Length; i++)
		{
			SetSectionColor(members[i], group.color);
			SetSectionGeneratorInherit(members[i], true);
			SetSectionSpawnInherit(members[i], true);
		}
	}

	public void AddSectionToGroup(SectionGroupData group, IntVector2 member)
	{
		if (group.members.Contains(member)) return;

		for (int i = 0; i < groups.Count; i++)
		{
			if (groups[i] == group) continue;
			if(groups[i].members.Contains(member))
			{
				groups[i].members.Remove(member);
				EventEditGroup.Invoke(groups[i]);
			}	
		}

		SetSectionGeneratorInherit(member, true);
		SetSectionSpawnInherit(member, true);

		group.members.Add(member);
		SetSectionColor(member, group.color);
		EventEditGroup.Invoke(group);
	}

	public void RemoveSectionFromGroup(IntVector2 member)
	{
		for(int i = 0; i < groups.Count; i++)
		{
			if(groups[i].members.Remove(member))
			{
				EventEditGroup.Invoke(groups[i]);
				if (groups[i].members.Count == 0)
					groups.RemoveAt(i);
				return;
			}
		}
	}

	public void SetGroupSpawner(SectionGroupData group, string spawntable)
	{
		if (groups.Contains(group) == false) return;

		group.spawntable = spawntable;
		foreach(IntVector2 iv2 in group.members)
		{
			SectionContainer container = null;
			if(sections.TryGetValue(iv2, out container))
			{
				if (container.inheritSpawnTable)
				{
					container.spawntable = spawntable;
					EventEditSection.Invoke(iv2, container);
				}
			}
		}
		EventEditGroup.Invoke(group);
	}

	public void SetGroupGenerator(SectionGroupData group, MapType terrain)
	{
		if (groups.Contains(group) == false) return;

		group.generator = terrain;
		foreach (IntVector2 iv2 in group.members)
		{
			SectionContainer container = null;
			if (sections.TryGetValue(iv2, out container))
			{
				if (container.inheritGenerator)
				{
					container.generator = terrain;
					EventEditSection.Invoke(iv2, container);
				}
			}
		}
		EventEditGroup.Invoke(group);
	}

	public void SetGroupColor(SectionGroupData group, Color color)
	{
		if (groups.Contains(group) == false) return;

		group.color = color;
		foreach (IntVector2 iv2 in group.members)
		{
			if (sections.ContainsKey(iv2))
				SetSectionColor(iv2, color);
		}
		EventEditGroup.Invoke(group);
	}

	public void SetGroupName(SectionGroupData group, string name)
	{
		if (groups.Contains(group) == false) return;

		group.groupName = name;

		foreach (IntVector2 iv2 in group.members)
		{
			SectionContainer container = null;
			if (sections.TryGetValue(iv2, out container))
			{
				EventEditSection.Invoke(iv2, container);
			}
		}

		EventEditGroup.Invoke(group);
	}

	public SectionGroupData GroupOf(IntVector2 section)
	{
		foreach(SectionGroupData group in groups)
		{
			if (group.members.Contains(section))
				return group;
		}
		return null;
	}

	public void DisbandGroup(SectionGroupData group)
	{
		groups.Remove(group);
		EventEditGroup.Invoke(group);
	}

	public void SetSectionModule(IntVector2 section, string module)
	{
		SectionContainer container = null;
		if(sections.TryGetValue(section, out container))
		{
			container.module = module;
		}
		EventEditSection.Invoke(section, container);
	}

	public void SetGroupModules(SectionGroupData group, params string[] modules)
	{
		group.modules = modules;
		EventEditGroup.Invoke(group);
	}

	public void SetGroupModuleCount(SectionGroupData group, int moduleCount)
	{
		group.moduleCount = moduleCount;
		EventEditGroup.Invoke(group);
	}

	[System.Serializable]
	public class SectionContainer
	{
		public bool inheritGenerator;
		public MapType generator;
		public string pregeneratedName;
		public bool inheritSpawnTable;
		public string spawntable;
		public CompassDirection connections;
		public string module = "";

		// editor attributes
		public SerializedColor color;
	}

	[System.Serializable]
	public class SectionGroupData
	{
		public string groupName = "new group";

		public List<IntVector2> members;

		public MapType generator;
		public string spawntable;

		public SerializedColor color = Color.white;
		public string editorname;
		public string[] modules = new string[0];
		public int moduleCount = 0;

		public SectionGroupData(params IntVector2[] members)
		{
			this.members = new List<IntVector2>(members);
		}

		public SectionGroupData()
		{
			members = new List<IntVector2>();
		}
	}


	//////////////////
	/// Seralization Stuffs
	///
	

	/// <summary>
	/// Only to be used by the XML seralizer.
	/// </summary>
	public KeyValuePair<IntVector2, SectionContainer>[] XMLSerializedSections
	{
		get
		{
			List<KeyValuePair<IntVector2, SectionContainer>> list = new List<KeyValuePair<IntVector2, SectionContainer>>();
			foreach(KeyValuePair<IntVector2, SectionContainer> pair in sections)
			{
				list.Add(pair);
			}

			list.Sort(comparer);

			return list.ToArray();
		}

		set
		{
            Debug.Log(value.Length);
            Debug.Log(value);
            for (int i = 0; i < value.Length; i++)
			{
                Debug.Log(value[i].Key + ": " + value[i].Value);
                sections[value[i].Key] = value[i].Value;
                // sections.Add(value[i].Key, value[i].Value);
			}
		}
	}

	/// <summary>
	/// A simple IComparer implementation to sort  sctions first by their x value and then by their y value.
	/// Used for seralization so that everything is always sorted the same way in the final document, purely for the benefit of git.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	private int comparer(KeyValuePair<IntVector2, SectionContainer> a, KeyValuePair<IntVector2, SectionContainer> b)
	{
		if (a.Key.x == b.Key.x)
			return a.Key.y - b.Key.y;
		return a.Key.x - b.Key.x;
	}



	public class SectionEvent : UnityEvent<IntVector2, SectionContainer> { }
	public class GroupEvent : UnityEvent<SectionGroupData> { }
}
