﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public int Radius;

	public InteractEvent EventInteract;
	public InteractEvent EventLeaveRadius;

	IntVector2 RealLocation
	{
		get
		{
			if (GetComponent<MapObject>())
				return GetComponent<MapObject>().RealLocation;
			return IntVector2.RoundFrom(transform.position);
		}
	}

	void Start()
	{

	}

	public bool Interact(MapObject user)
	{
		if (!enabled) return false;
		if(user.RealLocation.DeltaMax(RealLocation) <= Radius)
		{
			EventInteract.Invoke(user);
			StartCoroutine(LeaveRangeCheck(user));
			return true;
		}

		return false;
	}

	IEnumerator LeaveRangeCheck(MapObject user)
	{
		while(user.RealLocation.DeltaMax(RealLocation) <= Radius)
		{
			yield return new WaitForSeconds(0.1f);
		}
		EventLeaveRadius.Invoke(user);
	}

	[System.Serializable]
	public class InteractEvent : UnityEvent<MapObject> { }
}
