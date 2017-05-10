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
		this.condition = condition;
		this.action = action;
	}

	public bool Try()
	{
		return condition() && action();
	}
}
