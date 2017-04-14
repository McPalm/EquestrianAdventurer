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
		}
		else
		{
			enabled = false;
			Destroy(this);
		}
	}

	static public DurationAura StackOn(GameObject target, int duration, Stats stats, BaseAttributes attributes, string displayName)
	{
		if(!target.GetComponent<MapCharacter>())
		{
			return null;
		}

		foreach(DurationAura da in target.GetComponents<DurationAura>())
		{
			if(da.SameAs(displayName, stats, attributes))
			{
				da.duration += duration;
				return da;
			}
		}

		DurationAura aura = target.AddComponent<DurationAura>();
		aura.duration = duration;
		aura.stats = stats;
		aura.attributes = attributes;
		aura.displayName = displayName;
		target.GetComponent<MapCharacter>().Refresh();
		return aura;
	}

	public bool SameAs(string displayName, Stats stats, BaseAttributes attributes)
	{
		return displayName == this.displayName
			&& stats.Equals(this.stats)
			&& attributes.Equals(this.attributes);
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
