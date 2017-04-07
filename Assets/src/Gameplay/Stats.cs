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
	public DamageTypes damageTypes;

	public Stats(int hp, int hit, int dodge, int armor, int damage, int armorpen, DamageTypes damageTypes)
	{
		this.hp = hp;
		this.hit = hit;
		this.dodge = dodge;
		this.armor = armor;
		this.damage = damage;
		this.armorpen = armorpen;
		this.damageTypes = damageTypes;
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
		return new Stats(a.hp + b.hp, a.hit + b.hit, a.dodge + b.dodge, a.armor + b.armor, a.damage + b.damage, a.armorpen + b.armorpen, a.damageTypes | b.damageTypes);
	}

	public override string ToString()
	{
		return string.Format("hp {0}, hit {1}, dodge {2}, armor {3}, damage {4}, armorpen {5}, damageTypes{6}", hp, hit, dodge, armor, damage, armorpen, damageTypes);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="substring">put at 0 if you wish to start with a new row</param>
	/// <returns></returns>
	public string NeatStringSkipEmpty(int substring = 1)
	{
		string s = "";
		if (damage != 0) s += "\nDamage: " + damage;
		if (armorpen != 0) s += "\nArmor Penetration: " + armorpen;
		if (hit != 0) s += "\nHit: " + hit;
		if (dodge != 0) s += "\nDodge: " + dodge;
		if (armor != 0) s += "\nArmor: " + armor;
		if (hp != 0) s += "\nHealth: " + hp;
		return s.Substring(substring);
	}
}
