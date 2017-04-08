using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;

public class EnemyClick : MonoBehaviour {

	public MapObjectEvent EventClickMapObject;
	public CharacterEvent EventClickCharacter;
	public CharacterEvent EventRightClickCharacter;
	public UnityEvent EventEmptyClick;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
		{
			if (Blocked) return;
			MapObject[] os = ObjectMap.Instance.ObjectsAtLocation(IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition)));

			if (Input.GetMouseButtonDown(0))
			{
				MapObject topStack = null;

				foreach (MapObject o in os)
				{
					if (!o.VisibleToPlayer) continue;
					if (o.GetComponent<MapCharacter>())
					{
						EventClickMapObject.Invoke(o);
						EventClickCharacter.Invoke(o.GetComponent<MapCharacter>());
						return;
					}
					else
						topStack = o;
				}
				if (topStack)
					EventClickMapObject.Invoke(os[0]);
				else
					EventEmptyClick.Invoke();
			}
			else if (Input.GetMouseButtonDown(1))
			{

				foreach (MapObject o in os)
				{
					if (!o.VisibleToPlayer) continue;
					if (o.GetComponent<MapCharacter>())
					{
						EventRightClickCharacter.Invoke(o.GetComponent<MapCharacter>());
						return;
					}
				}
			}
		}
	}

	bool Blocked
	{
		get
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}

	[System.Serializable]
	public class MapObjectEvent : UnityEvent<MapObject> { }
	[System.Serializable]
	public class CharacterEvent : UnityEvent<MapCharacter> { }
}
