using UnityEngine;
using System;

[RequireComponent(typeof(LOSCheck))]
[RequireComponent(typeof(SimpleBehaviour))]
public class AgressiveMelee : MonoBehaviour
{
	public GameObject target;
	public int sightRadius = 10;

	IntVector2 home;

	void Start()
	{
		GetComponent<SimpleBehaviour>().startTurnEvent.AddListener(DoTurn);
		GetComponent<SimpleBehaviour>().endTurnEvent.AddListener(DoTurn);
		home = GetComponent<MapObject>().RealLocation;
		target = FindObjectOfType<RogueController>().gameObject; // wow
	}

	void DoTurn(SimpleBehaviour ai)
	{
		if (GetComponent<MapObject>().RealLocation.DeltaMax(target.GetComponent<MapObject>().RealLocation) > sightRadius)
		{
			ai.targetCharacter = null;
			return;
		}

		if (GetComponent<LOSCheck>().HasLOS(target.GetComponent<MapObject>()))
		{
			ai.targetCharacter = target.GetComponent<MapCharacter>();
			ai.targetLocation = target.GetComponent<MapObject>().RealLocation; // go to last location
		}
		else
		{
			ai.targetCharacter = null;
			if (GetComponent<MapObject>().RealLocation == ai.targetLocation) ai.targetLocation = home; // return to home if at last known location
		}
	}
}
