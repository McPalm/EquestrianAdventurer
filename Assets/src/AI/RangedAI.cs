using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

/// <summary>
/// The ranged enemy, the scirmisher, the bane of all players in roguelikes.
/// this enemy will keep their distance to the player at all times, fire/reload whenever possible and not in melee
/// Only fighting back if pushing into a corner
/// </summary>

[RequireComponent(typeof(LOSCheck))]
[RequireComponent(typeof(CharacterActionController))]
public class RangedAI : MonoBehaviour, TurnTracker.TurnEntry
{
	LOSCheck LOS;
	RangedAttack rangedAttack;
	MapObject me;
	CharacterActionController controller;

	IntVector2 home;

	public MapObject target;

	public int alertRadius = 9;
	public int relaxedRadius = 6;
	public bool scrimisher;

	IntVector2 lastSeenLocation;
	bool investigate = false;
	bool relaxed = true;
	bool seenPlayer;

	public UnityEvent PreTurnEvent = new UnityEvent();

	// Use this for initialization
	void Start ()
	{
		LOS = GetComponent<LOSCheck>();
		rangedAttack = GetComponent<RangedAttack>();
		me = GetComponent<MapObject>();
		controller = GetComponent<CharacterActionController>();
		home = me.RealLocation;
		target = FindObjectOfType<RogueController>().GetComponent<MapObject>();
		if (GetComponent<MapCharacter>().HostileTowards(target.GetComponent<MapCharacter>()) == false) target = null;

		TurnTracker.Instance.Add(this);
		GetComponent<MapCharacter>().EventDeath.AddListener(delegate { TurnTracker.Instance.Remove(this); });
		GetComponent<MapCharacter>().EventHearNoise.AddListener(OnHearNoise);

		LOS.sightRadius = relaxedRadius;

		GetComponent<HitPoints>().EventBeforeHurt.AddListener(OnHurt);
	}

	public void DoTurn()
	{
		PreTurnEvent.Invoke();
		bool acted = false;
		bool sight = target;
		if(target) sight = LOS.HasLOS(target, relaxed, true);
		if (sight)
		{
			if (!seenPlayer)
			{
				CombatTextPool.Instance.PrintAt((Vector3)me.RealLocation + new Vector3(0f, 0.4f), "!", Color.yellow, 1.2f);
				Yell();
			}
			seenPlayer = true;
			investigate = true;
			lastSeenLocation = target.RealLocation;
			relaxed = false;
			LOS.sightRadius = alertRadius;
		}
		if (relaxed)
		{
			if ((me.RealLocation - home).MagnitudePF > 2)
			{
				if (!MoveTowards(home)) RandomMove();
			}
			else if(UnityEngine.Random.value < 0.2f)
				RandomMove();
			return;
		}
		bool melee = target;
		if(target) melee = IntVector2Utility.DeltaSum(me.RealLocation, target.RealLocation) == 1;
		//int distance = IntVector2Utility.PFDistance(me.RealLocation, player.RealLocation);
		if(melee)
		{
			if (rangedAttack && scrimisher)
			{
				acted = MoveAway(target.RealLocation);
				if (acted) return;
			}
			acted = MoveTowards(target.RealLocation); // if we fail at running. melee the player
			if (acted) return;
		}
		if (target && rangedAttack)
		{
			if (sight && rangedAttack.Useable)
			{
				acted = rangedAttack.Attack(target.GetComponent<MapCharacter>()); // Bypasses CharacterActionController, fix this
				if (acted) return;
			}
			else if (rangedAttack is Crossbow && !rangedAttack.Useable)
			{
				(rangedAttack as Crossbow).Reload();
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
			if(!MoveTowards(home))
				RandomMove();
			if (  (me.RealLocation - home).MagnitudePF < 2)
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

	public bool Relaxed
	{
		get
		{
			return relaxed;
		}
	}

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
		Investigate(source);
	}

	void RandomMove()
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
			case 0:
				controller.Perform(Vector2.up); break;
			case 1:
				controller.Perform(Vector2.right); break;
			case 2:
				controller.Perform(Vector2.down); break;
			case 3:
				controller.Perform(Vector2.left); break;
		}
	}

	void OnHurt(DamageData damage)
	{
		if(!investigate && damage.source)
		{
			CombatTextPool.Instance.PrintAt((Vector3)me.RealLocation + new Vector3(0f, 0.4f), "!!", Color.red, 1.2f);
			Investigate(IntVector2.RoundFrom(damage.source.transform.position));
			Yell();
		}
	}

	void Investigate(IntVector2 location)
	{
		investigate = true;
		relaxed = false;
		lastSeenLocation = location;
		LOS.sightRadius = alertRadius;
	}
}

