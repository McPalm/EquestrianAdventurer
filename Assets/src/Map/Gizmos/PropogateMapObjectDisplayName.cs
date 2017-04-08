using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PropogateMapObjectDisplayName : MonoBehaviour
{

	public StringEvent EventSetDisplayName = new StringEvent();
	public UnityEvent EventFailSet = new UnityEvent();


	public void Set(MapObject o)
	{
		if (o)
		{
			EventSetDisplayName.Invoke(o.displayName);
		}
		else
			EventFailSet.Invoke();
	}

	public void Set(GameObject o)
	{
		Set(o.GetComponent<MapObject>());
	}

	public void Set(Component c)
	{
		Set(c.GetComponent<MapObject>());
	}

	[System.Serializable]
	public class StringEvent : UnityEvent<string> { }
}
