using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageData
{

	public DamageTypes damageType;

	// all fields need to be public in order to be serialized and sent over the network.
	public int damage = 0;

	public float multiplier = 1f;
	public int armorPenetration = 0;
	public bool critical = false;
	public GameObject source;

	public int TotalDamage
	{
		get
		{
			if(critical) return Mathf.RoundToInt(damage * multiplier * 2f);
			else return Mathf.RoundToInt(damage * multiplier);
		}
	}

	public DamageData(GameObject source)
	{
		this.source = source;
	}

	public DamageData SetDamage(int d)
	{
		damage = d;
		return this;
	}
	public DamageData SetDamage(float d)
	{
		damage = Mathf.RoundToInt(d);
		return this;
	}
	public DamageData SetArmorPen(int ap)
	{
		armorPenetration = ap;
		return this;
	}
	public DamageData SetCritical(bool crit)
	{
		critical = crit;
		return this;
	}

	public DamageData AddType(DamageTypes dt)
	{
		damageType = damageType | dt;
		if ((dt & (DamageTypes.piercing | DamageTypes.bludgeoning | DamageTypes.slashing)) != DamageTypes.untyped)
			damageType = damageType | DamageTypes.physical;
		return this;
	}

	public bool HasAllTypes(DamageTypes dt)
	{
		if (dt == 0) return dt == damageType;
		return (damageType & dt) == dt;
	}

	public bool HasAnyType(DamageTypes dt)
	{
		if (dt == 0) return dt == damageType;
		return (damageType & dt) != 0;
	}

	[System.Serializable]
	public class DamageEvent : UnityEvent<DamageData> { }
}
