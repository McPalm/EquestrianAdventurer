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

	public bool Gift(Item i)
	{
		if (giftTriggers.Count == 0) return false;
		foreach(StoryTriggersData.StoryTrigger st in giftTriggers)
		{
			foreach(string s in st.Keywords)
			{
				if(s.ToLower() == i.displayName.ToLower())
				{
					InvokeTrigger(st);
					return true;
				}
			}
		}
		return false;
	}

	void InvokeTrigger(StoryTriggersData.StoryTrigger st)
	{
		print("Trigger conditions met");
		switch(st.Action)
		{
			case StoryAction.createItem:
				Inventory playerInventory = FindObjectOfType<RogueController>().GetComponent<Inventory>();
				Item item = null;
				if (CreateItem.Instance.TryGet(st.String, out item))
					playerInventory.AddOrPutOnGround(item);
				else
					Debug.LogError("Unable to find " + st.String + " in Item Database");
				break;
		}
	}
}
