using UnityEngine;
using System.Collections;
using System;

public class Poison : Aura
{
	Stats stats;

	int duration;
	int outstandingDamage;
	float damagebuildup;
	float damageperround;

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
			if (outstandingDamage > 20) return "Badly Poisoned!!!\nTaking damage overtime." + stats.NeatStringSkipEmpty(0);
			else if (outstandingDamage > 20) return "Badly Poisoned!!\nTaking damage overtime." + stats.NeatStringSkipEmpty(0);
			else if (outstandingDamage > 10) return "Badly Poisoned!\nTaking damage overtime." + stats.NeatStringSkipEmpty(0);
			else return "Poisoned!\nTaking damage overtime." + stats.NeatStringSkipEmpty(0);
		}
	}

	static public void StackPoison(GameObject target, int duration, int damage, Sprite s)
	{
		Poison p = target.GetComponent<Poison>();
		if (!p) p = target.AddComponent<Poison>();
		if (p.duration < duration) p.duration = duration + p.duration / 2;
		else p.duration += duration / 2;
		p.outstandingDamage += damage;
		p.Calc();
		p.Icon = s;
	}

	void Calc()
	{
		CalcStats();
		damageperround = (float)outstandingDamage / duration;
	}

	void CalcStats()
	{
		stats.hit = -1 - (outstandingDamage / 6);
		stats.dodge = -1 - (outstandingDamage / 6);
		stats.hp = -1 - (outstandingDamage / 3);
		GetComponent<MapCharacter>().Refresh();
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
		damagebuildup += damageperround;
		if(damagebuildup >= 1f)
		{
			GetComponent<HitPoints>().Hurt(new DamageData().SetDamage(1).AddType(DamageTypes.poison));
			outstandingDamage--;
			damagebuildup -= 1f;
			CombatTextPool.Instance.PrintAt(transform.position, "-1", Color.green);
			CalcStats();
		}

		duration--;
		if (duration < 1)
		{
			GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnEndTurn);
			Destroy(this);
			GetComponent<MapCharacter>().Refresh();
		}
		
	}
}
