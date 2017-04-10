using UnityEngine;
using System.Collections;
using System;

public class DurationAura : Aura
{
	public string displayName;
	public int duration;
	public Stats stats;
	public BaseAttributes attributes;

	public override BaseAttributes Attributes
	{
		get
		{
			return attributes;
		}
	}

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

	public override string Tooltip
	{
		get
		{
			return displayName + stats.NeatStringSkipEmpty(0) + attributes.NeatStringSkipEmpty(0);
		}
	}

	// Use this for initialization
	protected void OnEnable()
	{
		CharacterActionController ac = GetComponent<CharacterActionController>();
		if(ac)
		{
			ac.EventAfterAction.AddListener(OnEndTurn);
			GetComponent<MapCharacter>().Refresh();
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
		if (duration < 1)
		{
			GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnEndTurn);
			Destroy(this);
			GetComponent<MapCharacter>().Refresh();
		}
	}
}
