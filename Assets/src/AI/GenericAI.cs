using UnityEngine;
using System.Collections.Generic;
using System;

public class GenericAI : MyBehaviour, TurnTracker.TurnEntry
{

	List<AINode> entries = new List<AINode>();
	protected CharacterActionController controller;

	public void DoTurn()
	{
		GameObject target = null;
		// aquire target code.

		if (target)
		{
			foreach (AINode node in entries)
			{
				if (node.Try()) return;
			}
		}
		else
		{
			// do the whole idle, search thing.
		}
	}
	
	void Awake()
	{
		GetComponent<MapCharacter>().EventDeath.AddListener(delegate { TurnTracker.Instance.Remove(this); });
		EventDisable.AddListener(delegate { TurnTracker.Instance.Remove(this); });
		controller = GetComponent<CharacterActionController>();
	}

	void OnEnable()
	{
		TurnTracker.Instance.Add(this);
	}

	/// <summary>
	/// Attack the target, with a melee attack if in close combat, otherwise with a ranged attack if it can.
	/// </summary>
	/// <param name="mc"></param>
	/// <returns></returns>
	protected bool Attack(MapCharacter mc)
	{
		Mobile me = GetComponent<Mobile>();
		Mobile them = mc.GetComponent<Mobile>();
		if (me.RealLocation.DeltaSum(them.RealLocation) == 1)
		{
			return GetComponent<CharacterActionController>().MoveTowards(them.RealLocation);
		}
		else if(me.RealLocation.DeltaSum(them.RealLocation) > 1 && GetComponent<RangedAttack>())
		{
			return GetComponent<RangedAttack>().Attack(mc);
		}
		return false;
	}
}
