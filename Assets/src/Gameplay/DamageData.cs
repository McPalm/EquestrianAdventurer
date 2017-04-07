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

	public int TotalDamage
	{
		get
		{
			return Mathf.RoundToInt(damage * multiplier);
		}
	}

	public DamageData SetDamage(int d)
	{
		damage = d;
		return this;
	}
	public DamageData SetArmorPen(int ap)
	{
		armorPenetration = ap;
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
