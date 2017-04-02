using UnityEngine;
using System.Collections;
using System;


public class DurationAura : Aura
{
	public string displayName;
	public int duration;
	public Stats stats;

	public override string IconText
	{
		get
		{
			return duration.ToString();
		}
	}

	public override Stats Stats
	{
		get
		{
			return stats;
		}
	}

	// Use this for initialization
	void OnEnable()
	{
		CharacterActionController ac = GetComponent<CharacterActionController>();
		if(ac)
		{
			ac.EventAfterAction.AddListener(OnEndTurn);
		}
		else
		{
			enabled = false;
			Destroy(this);
		}
	}

	void OnEndTurn(CharacterActionController cac, CharacterActionController.Actions a)
	{
		duration--;
		print(duration);
		if (duration < 1)
		{
			GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnEndTurn);
			Destroy(this);
			GetComponent<MapCharacter>().Refresh();
		}
	}
}
