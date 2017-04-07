using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HitPoints))]
public class Ghost : MyBehaviour
{

	void Awake()
	{
		EventDisable.AddListener(UnSubscribe);
	}

	void OnEnable()
	{
		Subscribe();
	}

	void Subscribe()
	{
		GetComponent<HitPoints>().EventBeforeHurt.AddListener(OnHurt);
	}

	void UnSubscribe()
	{
		GetComponent<HitPoints>().EventBeforeHurt.RemoveListener(OnHurt);
	}
	
	void OnHurt(DamageData d)
	{
		if(!d.HasAnyType(DamageTypes.ghostTouch)) d.multiplier *= 0.5f;
	}
}
