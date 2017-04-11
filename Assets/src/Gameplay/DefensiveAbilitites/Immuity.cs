using UnityEngine;
using System.Collections;

public class Immuity : MyBehaviour
{
	public DamageTypes immuneTo;

	// Use this for initialization
	void Start ()
	{
		EventDisable.AddListener(MyDisable);
	}

	void OnEnable()
	{
		GetComponent<HitPoints>().EventBeforeHurt.AddListener(OnHurt);
	}

	void OnHurt(DamageData d)
	{
		if (d.HasAnyType(immuneTo))
			d.multiplier = 0f;
	}

	void MyDisable()
	{
		GetComponent<HitPoints>().EventBeforeHurt.RemoveListener(OnHurt);
	}
}
