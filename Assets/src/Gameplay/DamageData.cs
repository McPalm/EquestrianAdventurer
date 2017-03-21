[System.Serializable]
public class DamageData
{

	public DamageTypes damageType;

	// all fields need to be public in order to be serialized and sent over the network.
	public int damage = 0;
	public int precision = 0;
	public int critical = 0;

	public int TotalDamage
	{
		get
		{
			return damage + precision;
		}
	}

	public DamageData SetDamage(int d)
	{
		damage = d;
		return this;
	}

	public DamageData SetPrecision(int d)
	{
		precision = d;
		return this;
	}

	public DamageData SetCritical(int d)
	{
		critical = d;
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
}
