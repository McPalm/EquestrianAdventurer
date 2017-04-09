using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StaminaPoints : MonoBehaviour
{
	public IntEvent EventChangeStamina = new IntEvent();
	public IntEvent EventChangeMaxStamina = new IntEvent();

	int current = 0;
	int max = 0;
	float staminaPerTurn;
	float recharge = 0f;

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
				if (max < current)
				{
					current = max;
					EventChangeStamina.Invoke(current);
				}

			}
		}
	}

	public int CurrentStamina
	{
		get
		{
			return current;
		}
		private set
		{
			if (value < 0) value = 0;
			if (value > max) value = max;
			if (current != value)
			{
				current = value;
				EventChangeStamina.Invoke(CurrentStamina);
			}
		}
	}

	public float StaminaPerTurn
	{
		get
		{
			return staminaPerTurn;
		}

		set
		{
			if (value < 0f) return;
			staminaPerTurn = value;
		}
	}

	public bool TryPay(int cost)
	{
		if (cost > CurrentStamina)
			return false;
		
		current -= cost;
		EventChangeStamina.Invoke(CurrentStamina);
		return true;
	}

	public void ForcePay(int cost)
	{
		CurrentStamina -= cost;
	}

	void Start()
	{
		CharacterActionController cac = GetComponent<CharacterActionController>();
		if(cac)
		{
			cac.EventAfterAction.AddListener(OnEndTurn);
		}
		EventChangeMaxStamina.Invoke(max);
		EventChangeStamina.Invoke(CurrentStamina);
	}

	void OnEndTurn(CharacterActionController cac, CharacterActionController.Actions a)
	{
		if (a == CharacterActionController.Actions.idle && current < max)
			CurrentStamina++;
		else
		{
			recharge += staminaPerTurn;
			if (recharge >= 1f)
			{
				CurrentStamina++;
				recharge -= 1f;
			}
		}
	}

	[System.Serializable]
	public class IntEvent : UnityEvent<int> { }
}
