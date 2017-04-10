using UnityEngine;
using System.Collections;

public class ForceFlagTest : MonoBehaviour {

	public string[] flags;

	// Use this for initialization
	void Start ()
	{
		foreach(string s in flags)
		{
			StoryFlags.Instance.AddFlag(s);
		}

	}
}
