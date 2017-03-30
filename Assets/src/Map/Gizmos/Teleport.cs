using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{


	public TeleportEvent EventTeleport;

	/// <summary>
	/// teleports the given object to the target location
	/// </summary>
	/// <param name="c"></param>
	/// <param name="targetLocation"></param>
	public void TeleportObject(Component c, GameObject targetLocation)
	{
		MapObject o = c.GetComponent<MapObject>();
		if(o)
		{
			o.Put(targetLocation.transform.position);
			EventTeleport.Invoke(o);
		}
	}

	/// <summary>
	/// Teleports the component to the location of this object
	/// </summary>
	/// <param name="c"></param>
	public void TeleportHere(Component c)
	{
		TeleportObject(c, gameObject);
	}

	[System.Serializable]
	public class TeleportEvent : UnityEvent<MapObject> { }
}
