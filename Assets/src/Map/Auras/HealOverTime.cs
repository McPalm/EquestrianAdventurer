using UnityEngine;
using System.Collections;
using System;

public class HealOverTime : Aura
{
	public int duration = 0;
	public int healFactor = 1;
	public string displayName;

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
			return new Stats();
		}
	}

	public override string Tooltip
	{
		get
		{
			return displayName + "\nHeals for " + healFactor + " hp per turn.";
		}
	}

	public override BaseAttributes Attributes
	{
		get
		{
			return new BaseAttributes();
		}
	}

	// Use this for initialization
	protected void OnEnable()
	{
		CharacterActionController ac = GetComponent<CharacterActionController>();
		if (ac)
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
		GetComponent<HitPoints>().Heal(new DamageData(gameObject).SetDamage(healFactor));
		duration--;
		if (duration < 1)
		{
			GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnEndTurn);
			Destroy(this);
		}
	}
}
