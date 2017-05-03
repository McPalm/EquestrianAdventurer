using UnityEngine;
using System.Collections;

public class Hurt : MonoBehaviour
{
	public int damage;
	public DamageTypes[] damageType;
	public bool damageNumbers;
	public GameObject source;

	public void HurtTarget(GameObject target)
	{
		if (source == null) source = gameObject;
		DamageTypes d = DamageTypes.untyped;
		foreach (DamageTypes dd in damageType)
			d |= dd;
		DamageData data = new DamageData(source)
			.AddType(d)
			.SetDamage(damage * Random.Range(0.75f, 1.25f));
		target.GetComponent<HitPoints>().Hurt(data);
		if (damageNumbers) HurtPool.Instance.DoHurt(IntVector2.RoundFrom(target.transform.position), data.TotalDamage);
	}

	public void HurtTarget(Component c)
	{
		HurtTarget(c.gameObject);
	}
}
