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
	public BaseAttributes attributes;
	[Space(10)]
	public int healovertime;
	public int healfactor;
	public bool idleOnly;
	[Space(10)]
	public bool returnScroll;

	public override Item CloneItem()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		ConsumeableFactory f = new ConsumeableFactory(label, baseValue, sr.sprite).SetColor(sr.color);

		if (healovertime > 0)
		{
			f.HealOverTime(healovertime, healfactor, idleOnly);
		}
		if (heal > 0) f.SetHeal(heal);
		if (buffDuration > 0) f.SetStatBoost(stats, buffDuration, attributes);
		if (returnScroll) f.Return();

		return f.Get();
	}


	static void Heal(GameObject target)
	{

	}
}
