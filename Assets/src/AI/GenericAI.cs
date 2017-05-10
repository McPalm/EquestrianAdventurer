using UnityEngine;
using System.Collections.Generic;
using System;

abstract public class GenericAI : MyBehaviour, TurnTracker.TurnEntry
{
	protected List<AINode> combatEntries = new List<AINode>();
	protected List<AINode> searchEntries = new List<AINode>();
	protected CharacterActionController controller;
	protected Action Idle;
	protected Func<bool> Combat;
	protected Func<bool> Search;

	public void DoTurn()
	{
		if (Combat())
		{
			foreach (AINode node in combatEntries)
			{
				if (node.Try()) return;
			}
			controller.Perform(CharacterActionController.Actions.idle);
		}
		else if(Search())
		{
			foreach(AINode node in searchEntries)
			{
				if (node.Try()) return;
			}
			controller.Perform(CharacterActionController.Actions.idle);
		}
		else
		{
			// do the whole idle, search thing.
			Idle();
		}
	}
	
	protected void Awake()
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
