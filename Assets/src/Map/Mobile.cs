﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Mobile : MapObject
{
	/// <summary>
	/// Event called whenever we move
	/// param 1 is desination
	/// param 2 is vector of movement
	/// </summary>
	public MovementEvent EventMovement = new MovementEvent();

	// Vector2 NetworkedPosition = new Vector2(999999999f, 99999999999f);

		/// <summary>
		/// Move the character in the given direction
		/// </summary>
		/// <param name="v2">direction of movement</param>
		/// <param name="bump">out parameter for a character that blocks movement, nullable</param>
		/// <returns>true if we moved, false if we could not move</returns>
	public bool MoveDirection(Vector2 v2, out MapCharacter bump)
	{
		// destination of move
		Vector2 d = (Vector2)RealLocation + v2;
		bump = null;

		// figure out if we can enter
		if (BlockMap.Instance.BlockMove(d)) return false;

		// check for other characters

		bump = ObjectMap.Instance.CharacterAt(IntVector2.RoundFrom(d));
		if (bump) return false;
		
		ForceMove(d);
		EventMovement.Invoke(d, v2);

		return true;
	}

	public void ForceMove(Vector2 v2)
	{
		//NetworkedPosition = new Vector2(Mathf.Round(v2.x), Mathf.Round(v2.y));
		OnMove(v2);
		RealLocation = IntVector2.RoundFrom(v2);
	}

	void OnMove(Vector2 v2)
	{
		StopAllCoroutines();
		float distance = ((Vector2)transform.position - v2).magnitude;
		
		StartCoroutine(Tween(transform.position, v2, distance/10f + 0.1f));
		
		// transform.position = v2;
	}

	IEnumerator Tween(Vector2 start, Vector2 destination, float duration)
	{
		if (duration > 0.4f) duration = 0.4f; // prevent slow moves
		float time = 0f;
		while(time < duration)
		{
			time += Time.deltaTime;
			transform.position = Vector2.Lerp(start, destination, time / duration);
			yield return new WaitForSeconds(0f);
		}
		transform.position = destination;
	}

	public class MovementEvent : UnityEvent<Vector2, Vector2> { };
}

