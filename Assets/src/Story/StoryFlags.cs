using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryFlags
{

	static StoryFlags _instance;

	HashSet<string> flags = new HashSet<string>();

	public static StoryFlags Instance
	{
		get
		{
			if (_instance == null) _instance = new StoryFlags();
			return _instance;
		}
	}

	public void AddFlag(string s)
	{
		if (flags.Add(s.ToLower()))
			Debug.Log("Adding story flag " + s);
	}

	public bool HasFlag(string s)
	{
		return flags.Contains(s.ToLower());
	}
}
