using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public int Radius;

	public UnityEvent EventInteract;

	public bool Interact(MapObject user)
	{
		if(user.RealLocation.DeltaMax(GetComponent<MapObject>().RealLocation) <= Radius)
		{
			EventInteract.Invoke();
			return true;
		}

		return false;
	}

	[System.Serializable]
	public class InteractEvent : UnityEvent<MapObject> { }
}
