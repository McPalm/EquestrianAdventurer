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
	public int critChance;
	public int critAvoid;
	public DamageTypes damageTypes;

	public Stats(int hp, int hit, int dodge, int armor, int damage, int armorpen, DamageTypes damageTypes, int critChance, int critAvoid)
	{
		this.hp = hp;
		this.hit = hit;
		this.dodge = dodge;
		this.armor = armor;
		this.damage = damage;
		this.armorpen = armorpen;
		this.damageTypes = damageTypes;
		this.critChance = critChance;
		this.critAvoid = critAvoid;
	}

	public float HitChance(Stats target)
	{
		if (target.dodge < 1 && hit < 1) return 0.5f;
		if (target.dodge < 1) return 1f;
		if (hit < 1 || hit * 9 < target.dodge) return 0.1f;
		return hit / (float)(hit + target.dodge);
	}

	public float CritChance(Stats target)
	{
		if (critChance < 1) return 0f;
		if (target.critAvoid < 0) return 0.25f;
		return (float)critChance / (critChance + target.critAvoid + 1) * 0.25f;

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
		return new Stats(a.hp + b.hp, a.hit + b.hit, a.dodge + b.dodge, a.armor + b.armor, a.damage + b.damage, a.armorpen + b.armorpen, a.damageTypes | b.damageTypes, a.critChance + b.critChance, a.critAvoid + b.critAvoid);
	}

	public override string ToString()
	{
		return string.Format("hp {0}, hit {1}, dodge {2}, armor {3}, damage {4}, armorpen {5}, damageTypes{6}, critChance{7}, critAvoid{8}", hp, hit, dodge, armor, damage, armorpen, damageTypes, critChance, critAvoid);
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
		if (critChance != 0) s += "\nCrit Chance: " + critChance;
		if (critAvoid != 0) s += "\nCrit Avoid: " + critAvoid;
		if (armor != 0) s += "\nArmor: " + armor;
		if (hp != 0) s += "\nHealth: " + hp;
		return s.Substring(substring);
	}
}
