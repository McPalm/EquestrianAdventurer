using UnityEngine;
using System.Collections;
using System;

public class Dash : AActiveAbility
{
	public int cost;
	public int range;

	public override bool CanUse
	{
		get
		{
			if(cost > 0 && GetComponent<StaminaPoints>())
				return GetComponent<StaminaPoints>().CurrentStamina >= cost;
			return true;
		}
	}

	public override bool CanUseAt(IntVector2 targetLocation)
	{
		return CanUse && GetComponent<LOSCheck>().HasLOE(targetLocation, range);	
	}

	public override bool TryUseAt(IntVector2 targetLocation)
	{
		if(CanUseAt(targetLocation))
		{
			if (GetComponent<Mobile>().CanEnter(targetLocation))
			{
				StaminaPoints s = GetComponent<StaminaPoints>();
				if (s)
				{
					if (!s.TryPay(cost)) return false;
				}
				GetComponent<Mobile>().ForceMove(targetLocation, 0.1f);
				if (GetComponent<SightRadius>())
					GetComponent<SightRadius>().RefreshView(range);
				return true;
			}
		}
		return false;
	}
}
