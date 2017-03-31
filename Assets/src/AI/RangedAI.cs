using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The ranged enemy, the scirmisher, the bane of all players in roguelikes.
/// this enemy will keep their distance to the player at all times, fire/reload whenever possible and not in melee
/// Only fighting back if pushing into a corner
/// </summary>

[RequireComponent(typeof(LOSCheck))]
public class RangedAI : MonoBehaviour, TurnTracker.TurnEntry
{
	LOSCheck LOS;
	Crossbow bow;
	MapObject me;
	CharacterActionController controller;

	IntVector2 home;

	public MapObject player;

	public int alertRadius = 9;
	public int relaxedRadius = 6;

	IntVector2 lastSeenLocation;
	bool investigate = false;
	bool relaxed = true;
	bool seenPlayer;

	// Use this for initialization
	void Start ()
	{
		LOS = GetComponent<LOSCheck>();
		bow = GetComponent<Crossbow>();
		me = GetComponent<MapObject>();
		controller = GetComponent<CharacterActionController>();
		home = me.RealLocation;
		player = FindObjectOfType<RogueController>().GetComponent<MapObject>();

		TurnTracker.Instance.Add(this);
		GetComponent<MapCharacter>().EventDeath.AddListener(delegate { TurnTracker.Instance.Remove(this); });
		GetComponent<MapCharacter>().EventHearNoise.AddListener(OnHearNoise);

		LOS.sightRadius = relaxedRadius;
		
	}

	public void DoTurn()
	{
		bool acted = false;
		bool sight = LOS.HasLOS(player, relaxed, true);
		if (sight)
		{
			if (!seenPlayer)
			{
				CombatTextPool.Instance.PrintAt((Vector3)me.RealLocation + new Vector3(0f, 0.4f), "!", Color.yellow, 1.2f);
				Yell();
			}
			seenPlayer = true;
			investigate = true;
			lastSeenLocation = player.RealLocation;
			relaxed = false;
			LOS.sightRadius = alertRadius;
		}
		if (relaxed) return;
		bool melee = IntVector2Utility.DeltaSum(me.RealLocation, player.RealLocation) == 1;
		//int distance = IntVector2Utility.PFDistance(me.RealLocation, player.RealLocation);
		if(melee)
		{
			if (bow)
			{
				acted = MoveAway(player.RealLocation);
				if (acted) return;
			}
			acted = MoveTowards(player.RealLocation); // if we fail at running. melee the player
			if (acted) return;
		}
		if (bow)
		{
			if (sight && bow.Loaded)
			{
				acted = bow.Attack(player.GetComponent<MapCharacter>()); // Bypasses CharacterActionController, fix this
				if (acted) return;
			}
			else if (!bow.Loaded)
			{
				bow.Reload();
				return;
			}
		}
		if (investigate)
		{
			MoveTowards(lastSeenLocation);
			if (me.RealLocation == lastSeenLocation) investigate = false;
		}
		else
		{
			MoveTowards(home);
			if(me.RealLocation == home)
			{
				relaxed = true;
				seenPlayer = false;
				LOS.sightRadius = relaxedRadius;
			}
		}

	}

	public bool MoveAway(IntVector2 destination)
	{
		bool moved = true;

		Vector2 delta = (Vector2)destination - (Vector2)me.RealLocation;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			if (delta.x < 0f) moved = controller.Perform(Vector2.right);
			else if (delta.x > 0f) moved = controller.Perform(Vector2.left);
			if (!moved)
			{
				if (delta.y < 0f) moved = controller.Perform(Vector2.up);
				else if (delta.y > 0f) moved = controller.Perform(Vector2.down);
				else moved = controller.Perform((UnityEngine.Random.value < 0.5f) ? Vector2.down : Vector2.up);
			}

		}
		else if (delta.y != 0f)
		{

			if (delta.y < 0f) moved = controller.Perform(Vector2.up);
			else if (delta.y > 0f) moved = controller.Perform(Vector2.down);
			if (!moved)
			{
				if (delta.x < 0f) moved = controller.Perform(Vector2.right);
				else if (delta.x > 0f) moved = controller.Perform(Vector2.left);
				else moved = controller.Perform((UnityEngine.Random.value < 0.5f) ? Vector2.right : Vector2.left);
			}
		}

		return moved;
	}

	public bool MoveTowards(IntVector2 destination)
	{
		bool moved = true;

		Vector2 delta = (Vector2)destination - (Vector2)me.RealLocation;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
		{
			if (delta.x < 0f) moved = controller.Perform(Vector2.left);
			else if (delta.x > 0f) moved = controller.Perform(Vector2.right);
			if (!moved)
			{
				if (delta.y < 0f) moved = controller.Perform(Vector2.down);
				else if (delta.y > 0f) moved = controller.Perform(Vector2.up);
				else moved = controller.Perform((UnityEngine.Random.value < 0.5f) ? Vector2.up : Vector2.down);
			}

		}
		else if (delta.y != 0f)
		{

			if (delta.y < 0f) moved = controller.Perform(Vector2.down);
			else if (delta.y > 0f) moved = controller.Perform(Vector2.up);
			if (!moved)
			{
				if (delta.x < 0f) moved = controller.Perform(Vector2.left);
				else if (delta.x > 0f) moved = controller.Perform(Vector2.right);
				else moved = controller.Perform((UnityEngine.Random.value < 0.5f) ? Vector2.left : Vector2.right);
			}
		}

		return moved;
	}

	void OnApplicationQuit()
	{
		teardown = true;
	}
	bool teardown = false;
	void OnDisable()
	{
		if (teardown) return;
		TurnTracker.Instance.Remove(this);
	}

	void Yell()
	{
		NoiseUtility.CauseNoise(4, me.RealLocation);
	}

	void OnHearNoise(IntVector2 source, int volume)
	{
		if (source == me.RealLocation) return;
		if (!investigate) CombatTextPool.Instance.PrintAt((Vector3)me.RealLocation + new Vector3(0f, 0.4f), "?", Color.yellow, 1.2f);
		lastSeenLocation = source;
		investigate = true;
		relaxed = false;
		LOS.sightRadius = alertRadius;
	}
}
