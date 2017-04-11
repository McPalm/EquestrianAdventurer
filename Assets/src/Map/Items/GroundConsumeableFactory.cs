using UnityEngine;
using System.Collections;

public class GroundConsumeableFactory : GroundItem
{
	public string label;
	public int heal = 0;
	public int baseValue;
	[Space(10)]
	public int buffDuration;
	public Stats stats;
	[Space(10)]
	public int healovertime;
	public int healfactor;
	public bool idleOnly;

	public override Item CloneItem()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		ConsumeableFactory f = new ConsumeableFactory(label, baseValue, sr.sprite).SetColor(sr.color);

		if (healovertime > 0)
		{
			f.HealOverTime(healovertime, healfactor, idleOnly);
		}
		if (heal > 0) f.SetHeal(heal);
		if (buffDuration > 0) f.SetStatBoost(stats, buffDuration);

		return f.Get();
	}


	static void Heal(GameObject target)
	{

	}
}
