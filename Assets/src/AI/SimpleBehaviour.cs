using UnityEngine;
using System.Collections;

/// <summary>
/// give it a target and it will move to the target and bump it.
/// </summary>
[RequireComponent(typeof(MapCharacter))]
public class SimpleBehaviour : MonoBehaviour, TurnTracker.TurnEntry
{
	public GameObject target;

	/// <summary>
	/// called when the AI is given an action
	/// </summary>
	public void DoTurn()
	{
		Vector2 delta = target.transform.position - transform.position;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			if (delta.x < 0f) MoveDirection(Vector2.left);
			else if (delta.x > 0f) MoveDirection(Vector2.right);
		}
		else if(delta.y != 0f)
		{
			if (delta.y < 0f) MoveDirection(Vector2.down);
			else if (delta.y > 0f) MoveDirection(Vector2.up);
		}
	}

	void MoveDirection(Vector2 v2)
	{
		MapCharacter mc = null;
		GetComponent<Mobile>().MoveDirection(v2, out mc);
		if (mc)
			GetComponent<MapCharacter>().Melee(mc);
	}

	void Start()
	{
		TurnTracker.Instance.Add(this);
	}

	void OnDisable()
	{
		TurnTracker.Instance.Remove(this);
	}
}
