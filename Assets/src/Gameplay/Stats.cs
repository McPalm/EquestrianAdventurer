using UnityEngine;
using System.Collections;

public struct Stats
{
	public int hp;
	public int hit;
	public int dodge;
	public int armor;
	public int damage;
	public int armorpen;

	public float BaseHitChance(Stats target)
	{
		return hit / (float)(hit + target.dodge);
	}

	public int DamageVersus(Stats target, float multiplier = 1f, float flatBonus = 0f)
	{
		return (int)((damage * target.DamageReduction(armorpen) * multiplier) + flatBonus);
	}

	public float DamageReduction(int armorpen)
	{
		if(armorpen < -8)
		{
			armorpen = -8;
		}
		return (10f + armorpen) / (10f + armorpen + armor);
	}
}
