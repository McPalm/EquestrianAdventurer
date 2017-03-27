using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// a draggable, but it can be dropped in an droparea
public class Dropable : Draggable
{
	Vector2 localStart;
	public float sortValue;

	public DropInAreaEvent EventDropInArea = new DropInAreaEvent();
	public DropableEvent EventDropOutside = new DropableEvent(); // called when not dropping over anything
	public DropableEvent EventDisable = new DropableEvent();
	

	protected void Start()
	{
		EventStartDrag.AddListener(Pickup);
		EventStopDrag.AddListener(Drop);
	}

	void Pickup(GameObject o)
	{
		localStart = target.localPosition;
	}

	void Drop(GameObject o)
	{
		// find if we have a dropzone underneath
		List<RaycastResult> results = new List<RaycastResult>();
		PointerEventData pointerData = new PointerEventData(EventSystem.current)
		{
			pointerId = -1,
		};
		pointerData.position = Input.mousePosition;

		EventSystem.current.RaycastAll(pointerData, results);

		if(results.Count == 1)
		{
			EventDropOutside.Invoke(this);
			return;
		}

		foreach(RaycastResult result in results)
		{
			DropArea a = result.gameObject.GetComponent<DropArea>();
			if(a)
			{
				if (a.Drop(this))
				{
					EventDropInArea.Invoke(this, a);
					return;
				}
			}
		}

		// if we dont, return
		StartCoroutine(CenterAt(localStart));
	}

	IEnumerator CenterAt(Vector2 destination)
	{
		Vector2 start = target.localPosition;
		for(float progress = 0; progress < 1f; progress += Time.deltaTime * 6f)
		{
			target.localPosition = Vector2.Lerp(start, destination, progress);
			yield return new WaitForSeconds(0f);
		}
		target.localPosition = destination;
	}

	void OnDisable()
	{
		EventDisable.Invoke(this);
	}

	public class DropInAreaEvent : UnityEvent<Dropable, DropArea> { }
	public class DropableEvent : UnityEvent<Dropable> { }
}
