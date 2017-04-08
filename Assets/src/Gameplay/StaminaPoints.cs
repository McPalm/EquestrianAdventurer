using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StaminaPoints : MonoBehaviour
{
	public IntEvent EventChangeStamina = new IntEvent();
	public IntEvent EventChangeMaxStamina = new IntEvent();

	int lost = 0;
	int max = 0;


	public int MaxStamina
	{
		get
		{
			return max;
		}
		set
		{
			if(value != max)
			{
				max = value;
				EventChangeMaxStamina.Invoke(max);
			}
		}
	}

	public int CurrentStamina
	{
		get
		{
			return max - lost;
		}
		private set
		{
			if (value < 0) value = 0;
			if (value > max) value = max;
			if (max - lost != value)
			{
				lost = max - value;
				EventChangeStamina.Invoke(CurrentStamina);
			}
		}
	}

	public bool TryPay(int cost)
	{
		if (cost > CurrentStamina)
			return false;
		
		lost += cost;
		EventChangeStamina.Invoke(CurrentStamina);
		return true;
	}

	public void ForcePay(int cost)
	{
		CurrentStamina -= cost;
	}

	[System.Serializable]
	public class IntEvent : UnityEvent<int> { }
}
