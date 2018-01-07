using UnityEngine;
using System.Collections;

[System.Serializable]
#pragma warning disable
public struct Stats
#pragma warning restore
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

    /// <summary>
    /// A number between 1 and 20, you need to roll at least this high on a d20 to hit the target
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public int HitNumber(Stats target)
    {
        int n = 10 - hit + target.dodge;
        if (n > 13) n = n / 2 + 6;
        n = System.Math.Min(20, n);
        return n;
    }

    /// <summary>
    /// A number between 1 and 20, you need to roll at least this high on a d20 to critically hit the target
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public float CritNumber(Stats target)
    {
        if (critChance <= 0) return 21; // cant crit
        if (target.critAvoid / 2 > critChance) return 21; // cant crit
        int c = critChance - target.critAvoid;
        if (c < 0) return 20;
        if (target.critAvoid <= 0) return 19 - c;
        int factor = critChance * 2 / target.critAvoid; // how many crit chance x2 can we fit into crit avoid.
        return (c < factor) ? 19 - c : 19 - factor;

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

	static public bool operator ==(Stats a, Stats b)
	{
		return a.damage == b.damage
			&& a.armorpen == b.armorpen
			&& a.critAvoid == b.critAvoid
			&& a.critChance == b.critChance
			&& a.armor == b.armor
			&& a.damageTypes == b.damageTypes
			&& a.dodge == b.dodge
			&& a.hit == b.hit
			&& a.hp == b.hp;
	}

	static public bool operator !=(Stats a, Stats b)
	{
		return !(a == b);
	}
}
