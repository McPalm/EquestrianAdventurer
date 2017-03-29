using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public int Radius;

	public UnityEvent EventInteract;
	public UnityEvent EventLeaveRadius;

	public bool Interact(MapObject user)
	{
		if(user.RealLocation.DeltaMax(GetComponent<MapObject>().RealLocation) <= Radius)
		{
			EventInteract.Invoke();
			StartCoroutine(LeaveRangeCheck(user));
			return true;
		}

		return false;
	}

	IEnumerator LeaveRangeCheck(MapObject user)
	{
		while(user.RealLocation.DeltaMax(GetComponent<MapObject>().RealLocation) <= Radius)
		{
			yield return new WaitForSeconds(0.1f);
		}
		EventLeaveRadius.Invoke();
	}

	[System.Serializable]
	public class InteractEvent : UnityEvent<MapObject> { }
}
