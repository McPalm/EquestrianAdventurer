using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StoryTriggersData
{
	static public string PATH = "XML/storytriggers/";

	public List<StoryTrigger> triggers = new List<StoryTrigger>();

	public void Save(string file)
	{
		XmlTool.EditorSaveObjectAsXML(this, PATH + file);
	}

	static public StoryTriggersData Load(string file)
	{
		return XmlTool.LoadFromXML<StoryTriggersData>(PATH + file);
	}

	static public StoryTriggersData LoadOrCreate(string file)
	{
		StoryTriggersData r = XmlTool.LoadFromXML<StoryTriggersData>(PATH + file, false);
		if(r == null)
		{
			r = new StoryTriggersData();
			StoryTrigger s = new StoryTrigger();
			s.Keywords = new string[] { "trigger" };
			r.triggers.Add(s);
		}
		return r;
	}

	[System.Serializable]
	public class StoryTrigger
	{
		public StoryTiming Timing;
		public string[] Keywords;
		public StoryAction Action;
		public float Number;
		public string String;
	}
}
