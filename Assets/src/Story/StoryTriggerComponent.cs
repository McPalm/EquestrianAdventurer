using UnityEngine;
using System.Collections.Generic;

public class StoryTriggerComponent : MonoBehaviour
{
	public StoryTriggersData data;

	List<StoryTriggersData.StoryTrigger> giftTriggers = new List<StoryTriggersData.StoryTrigger>();

	void Start()
	{
		foreach(StoryTriggersData.StoryTrigger st in data.triggers)
		{
			switch(st.Timing)
			{
				case StoryTiming.load:
					CheckTrigger(st);
					break;
				case StoryTiming.gift:
					giftTriggers.Add(st);
					break;
			}
		}
	}

	void CheckTrigger(StoryTriggersData.StoryTrigger st)
	{
		bool pass = true;
		foreach(string s in st.Keywords)
		{
			if (!StoryFlags.Instance.HasFlag(s)) pass = false;
		}
		if (pass)
			InvokeTrigger(st);
	}

	public void Gift(Item i)
	{
		if (giftTriggers.Count == 0) return;
		foreach(StoryTriggersData.StoryTrigger st in giftTriggers)
		{
			foreach(string s in st.Keywords)
			{
				if(s.ToLower() == i.displayName.ToLower())
				{
					InvokeTrigger(st);
					return;
				}
			}
		}
		
	}

	void InvokeTrigger(StoryTriggersData.StoryTrigger st)
	{
		print("Trigger successfully triggered!");
	}
}
