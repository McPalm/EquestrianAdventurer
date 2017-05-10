using UnityEngine;
using System.Collections;

public class ScriptedAI : GenericAI
{
	MapCharacter target;
	bool alert = false;
	bool atHome = true;

	IntVector2 home;
	IntVector2 lastSeen; // if the target is visible, last seen is the targets current location.

	// Use this for initialization
	new void Awake()
	{
		base.Awake();
		Combat = FindTarget;

		// hack some default AI
		// attack target
		combatEntries.Add(new AINode(null, AttackTarget));
		// approach target
		combatEntries.Add(new AINode(null, ApproachLastSeenLocation));

		Search = Looking;
		searchEntries.Add(new AINode(StillLooking, ApproachLastSeenLocation));
		searchEntries.Add(new AINode(null, GoHome));

		Idle = Wander;
		home = IntVector2.RoundFrom(transform.position);
	}

	bool AttackTarget()
	{
		atHome = false;
		return Attack(target);
	}

	bool ApproachLastSeenLocation()
	{
		atHome = false;
		if (controller.MoveTowards(lastSeen))
		{
			if (!target)
				FindTarget();
			return true;
		}
		else return false;
	}

	bool StillLooking()
	{
		if (alert)
			return (GetComponent<MapObject>().RealLocation != lastSeen);
		else
			return false;
	}

	bool GoHome()
	{
		alert = false;
		controller.MoveTowards(home); // do some fancy failsafe later, like, start pathing if failing the movetowards
		if (GetComponent<MapObject>().RealLocation == home) atHome = true;
		return true;
		
	}

	bool Looking()
	{
		return !atHome;
	}

	bool FindTarget()
	{
		if(target)
		{
			// verify LOS
			if (false == GetComponent<LOSCheck>().HasLOS(target.GetComponent<Mobile>()))
				target = null;
			else
				lastSeen = target.GetComponent<MapObject>().RealLocation;
		}
		if(!target)
		{
			IntVector2 realLocation = GetComponent<Mobile>().RealLocation;
			int bestdelta = 10;

			// find a (new) target
			foreach (MapObject o in ObjectMap.Instance.GetRange(realLocation.x - 8, realLocation.y - 8, realLocation.x + 8, realLocation.y + 8))
			{
				MapCharacter mc = o.GetComponent<MapCharacter>();
				if(mc && GetComponent<MapCharacter>().HostileTowards(mc) && GetComponent<LOSCheck>().HasLOS(o))
				{

					if ((realLocation - o.RealLocation).MagnitudePF < bestdelta)
					{
						target = mc;
						lastSeen = o.RealLocation;
						bestdelta = (realLocation - o.RealLocation).MagnitudePF;
					}	
				}
			}
		}


		if (target)
		{
			alert = true;
			return true;
		}
		return false;
	}



	void Wander()
	{
		// % chance we move in a random direction
		// % chance we move towards home
		if(Random.value < 0.15f)
		{
			switch(Random.Range(0, 4))
			{
				case 0: controller.Perform(Vector2.up); break;
				case 1: controller.Perform(Vector2.right); break;
				case 2: controller.Perform(Vector2.down); break;
				case 3: controller.Perform(Vector2.left); break;
			}
		}
		else if(Random.value < 0.15f)
		{
			controller.MoveTowards(home);
		}

	}

}