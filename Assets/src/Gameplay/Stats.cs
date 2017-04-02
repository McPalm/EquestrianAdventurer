using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Stats
{
	public int hp;
	public int hit;
	public int dodge;
	public int armor;
	public int damage;
	public int armorpen;

	public Stats(int hp, int hit, int dodge, int armor, int damage, int armorpen)
	{
		this.hp = hp;
		this.hit = hit;
		this.dodge = dodge;
		this.armor = armor;
		this.damage = damage;
		this.armorpen = armorpen;
	}

	public float HitChance(Stats target)
	{
		return hit / (float)(hit + target.dodge);
	}

	public float HitChance(Stats target, int hitbonus, int dodgebonus)
	{
		return (hit + hitbonus) / (float)(hit + hitbonus + target.dodge + dodgebonus);
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

	static public Stats operator + (Stats a, Stats b)
	{
		return new Stats(a.hp + b.hp, a.hit + b.hit, a.dodge + b.dodge, a.armor + b.armor, a.damage + b.damage, a.armorpen + b.armorpen);
	}

	public override string ToString()
	{
		return string.Format("hp {0}, hit {1}, dodge {2}, armor {3}, damage {4}, armorpen {5}", hp, hit, dodge, armor, damage, armorpen);
	}
}
