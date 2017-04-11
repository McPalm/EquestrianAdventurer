using UnityEngine;
using System.Collections;
using System;

public class HealOverTime : Aura
{
	public int duration = 0;
	public int healFactor = 1;
	public string displayName;
	public bool idleOnly = false;
	bool interrupt = false;

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
			GetComponent<HitPoints>().EventBeforeHurt.AddListener(OnHurt);
			
		}
		else
		{
			enabled = false;
			Destroy(this);
		}
	}

	void OnEndTurn(CharacterActionController cac, CharacterActionController.Actions a)
	{
		HitPoints hp = GetComponent<HitPoints>();
		hp.Heal(new DamageData(gameObject).SetDamage(healFactor));
		duration--;
		if (idleOnly && hp.CurrentHealth == hp.MaxHealth) interrupt = true;
		if (interrupt || duration < 1)
		{
			GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnEndTurn);
			Destroy(this);
			if (idleOnly)
			{
				GetComponent<HitPoints>().EventBeforeHurt.RemoveListener(OnHurt);
			}
		}
		else if(idleOnly)
		{
			cac.StackAction(CharacterActionController.Actions.idle);
		}
	}

	void OnHurt(DamageData d)
	{
		if (!idleOnly) return;
		if(d.damageType == DamageTypes.poison)
		{
			// nothing
		}
		else
		{
			print("hurt, interrupt!");
			interrupt = true;
		}
	}
}
