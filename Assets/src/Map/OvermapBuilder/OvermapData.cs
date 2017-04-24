using UnityEngine;
using UnityEngine.Events;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class OvermapData
{
	public Dictionary<IntVector2, SectionContainer> sections = new Dictionary<IntVector2, SectionContainer>();
	public List<SectionGroupData> groups = new List<SectionGroupData>();

	public SectionEvent EventEditSection = new SectionEvent();
	public GroupEvent EventEditGroup = new GroupEvent();

	public SectionContainer AddSection(IntVector2 location)
	{
		SectionContainer ret = new SectionContainer();
		sections.Add(location, ret);
		ret.color = Color.HSVToRGB(Random.value, Random.Range(0.7f, 1f), Random.Range(0.5f, 1f));
		EventEditSection.Invoke(location, ret);
		return ret;
	}

	public void SetSectionGenerator(IntVector2 iv2, MapType type, string name, bool inherit)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.generator = type;
			sc.inheritGenerator = inherit;
			sc.pregeneratedName = name;

			EventEditSection.Invoke(iv2, sc);
		}
	}

	public void SetSectionSpawntable(IntVector2 iv2, string table, bool inherit)
	{
		SectionContainer sc = null;
		if (sections.TryGetValue(iv2, out sc))
		{
			sc.spawntable = table;
			sc.inheritSpawnTable = inherit;

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
		SectionGroupData group = new SectionGroupData(members);
		groups.Add(group);
		EventEditGroup.Invoke(group);
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

		group.members.Add(member);
		EventEditGroup.Invoke(group);
	}

	public void RemoveSectionFromGroup(IntVector2 member)
	{
		for(int i = 0; i < groups.Count; i++)
		{
			if(groups[i].members.Remove(member))
			{
				EventEditGroup.Invoke(groups[i]);
				return;
			}
		}
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

		// editor attributes
		public SerializedColor color;
	}

	[System.Serializable]
	public class SectionGroupData
	{
		public List<IntVector2> members;

		public MapType generator;
		public string spawntable;

		public SerializedColor color = Color.white;
		public string editorname;

		public SectionGroupData(params IntVector2[] members)
		{
			this.members = new List<IntVector2>(members);
		}

		public SectionGroupData()
		{
			members = new List<IntVector2>();
		}
	}

	public class SectionEvent : UnityEvent<IntVector2, SectionContainer> { }
	public class GroupEvent : UnityEvent<SectionGroupData> { }
}
