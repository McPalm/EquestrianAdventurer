using UnityEngine;
using System.Collections;
using System;

public class Dash : AActiveAbility
{
	public int cost;
	public int range;
	[SerializeField]
	Sprite icon;

	public override bool CanUse
	{
		get
		{
			if(cost > 0 && Stamina)
				return Stamina.CurrentStamina >= cost;
			return true;
		}
	}

	public override Sprite Icon
	{
		get
		{
			return icon;
		}
	}

	public override bool CanUseAt(IntVector2 targetLocation)
	{
		return CanUse && LOS.HasLOE(targetLocation, range);	
	}

	public override bool TryUseAt(IntVector2 targetLocation)
	{
		if(CanUseAt(targetLocation))
		{
			if (Me.CanEnter(targetLocation))
			{
				
				if (Stamina)
				{
					if (!Stamina.TryPay(cost)) return false;
				}
				Me.ForceMove(targetLocation, 0.1f);
				if (User.GetComponent<SightRadius>())
					User.GetComponent<SightRadius>().RefreshView(range);
				return true;
			}
		}
		return false;
	}
}
