﻿using UnityEngine;
using UnityEngine.Events;

public class HitPoints : MonoBehaviour
{
	int maxHealth;
	int damageTaken;
	
	public HealthEvent EventChangeHealth;
	public DamageData.DamageEvent EventBeforeHurt;
	public DamageData.DamageEvent EventBeforeHeal;

	void Start()
	{
		EventChangeHealth.Invoke(CurrentHealth, MaxHealth);
	}

	public int CurrentHealth
	{
		get
		{
			return maxHealth - damageTaken;
		}
		set
		{
			if (maxHealth - value < 0)
				damageTaken = 0;
			else
				damageTaken = maxHealth - value;
			EventChangeHealth.Invoke(maxHealth - damageTaken, maxHealth);
		}
	}

	public float HealthPercent
	{
		get
		{
			return CurrentHealth / (float)maxHealth;
		}
	}
	public int MaxHealth
	{
		get
		{
			return maxHealth;
		}

		set
		{
			maxHealth = value;
			EventChangeHealth.Invoke(maxHealth - damageTaken, maxHealth);
		}
	}

	public void Hurt(DamageData d)
	{
		EventBeforeHurt.Invoke(d);
		damageTaken += d.TotalDamage;
		EventChangeHealth.Invoke(maxHealth - damageTaken, maxHealth);
	}
	
	public void Heal(DamageData d)
	{
		EventBeforeHeal.Invoke(d);
		CurrentHealth += d.TotalDamage;	
	}

	void OnChangeMax(int max)
	{
		EventChangeHealth.Invoke(max - damageTaken, max);
	}

	[System.Serializable]
	public class HealthEvent : UnityEvent<int, int> { }
	
}
