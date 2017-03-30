using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Just a bored idle AI that shuffles around every so often
/// </summary>
[RequireComponent(typeof(MapCharacter))]
public class IdleAI : MonoBehaviour, TurnTracker.TurnEntry
{
	CharacterActionController controller;

	public int movementRadius = 3;
	IntVector2 home;

	public void DoTurn()
	{
		if(UnityEngine.Random.value < 0.2f)
		{
			Shuffle();
		}
	}

	void Shuffle()
	{
		for (int i = 0; i < 5; i++)
		{
			int rand = UnityEngine.Random.Range(0, 4);
			if (rand == 0 && home.x + movementRadius > GetComponent<MapObject>().RealLocation.x)
				if(MoveDirection(Vector2.right)) i = 5;
			if (rand == 1 && home.x - movementRadius < GetComponent<MapObject>().RealLocation.x)
				if (MoveDirection(Vector2.left)) i = 5;
			if (rand == 2 && home.y + movementRadius > GetComponent<MapObject>().RealLocation.y)
				if (MoveDirection(Vector2.up)) i = 5;
			if (rand == 3 && home.y - movementRadius < GetComponent<MapObject>().RealLocation.y)
				if (MoveDirection(Vector2.down)) i = 5;
		}
	}

	// Use this for initialization
	void Start ()
	{
		TurnTracker.Instance.Add(this);
		home = GetComponent<MapObject>().RealLocation;
		GetComponent<MapCharacter>().EventDeath.AddListener(delegate { TurnTracker.Instance.Remove(this); });
	}

	bool MoveDirection(Vector2 v2)
	{
		MapCharacter mc = null;
		if (GetComponent<Mobile>().MoveDirection(v2, out mc))
			return true;
		return false;
	}
}