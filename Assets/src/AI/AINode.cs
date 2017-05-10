using UnityEngine;
using System.Collections;

public class AINode
{
	public delegate bool Condition();
	public delegate bool Action();

	Condition condition;
	Action action;

	public AINode(Condition condition, Action action)
	{
		if (condition == null) condition = autoPass;
		this.condition = condition;
		this.action = action;
	}

	public bool Try()
	{
		return condition() && action();
	}

	bool autoPass()
	{
		return true;
	}
}
