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
	Transform parentStart;
	public float sortValue;
	/// <summary>
	/// if disabled, can only move this between containers using function calls rather than dropping.
	/// </summary>
	public bool autoMove;
	DropArea current;
	Vector3 pickupMousePosition;
	float pickupTime;
	

	public DropInAreaEvent EventDropInArea = new DropInAreaEvent();
	public DropableEvent EventDropOutside = new DropableEvent(); // called when not dropping over anything
	public DropableEvent EventClick = new DropableEvent(); // called if we recognize a click instead of a drop.

	protected void Start()
	{
		EventStartDrag.AddListener(Pickup);
		EventStopDrag.AddListener(Drop);
	}

	void Pickup(GameObject o)
	{
		localStart = target.localPosition;
		parentStart = transform.parent;

		// put at an overlay canvass
		if (DropCanvas.Canvas) transform.SetParent(DropCanvas.Canvas);
		pickupMousePosition = Input.mousePosition;
		pickupTime = Time.time;
	}

	void Drop(GameObject o)
	{
		// return to the right canvass
		transform.SetParent(parentStart);

		if ((Input.mousePosition - pickupMousePosition).magnitude < 10f)
		{
			Return();
			if (Time.time - pickupTime < 0.2f)
			{
				if (current) current.EventClick.Invoke(this);
				EventClick.Invoke(this);
			}
			return;
		}
		// find if we have a dropzone underneath
		List<RaycastResult> results = new List<RaycastResult>();
		PointerEventData pointerData = new PointerEventData(EventSystem.current)
		{
			pointerId = -1,
		};
		pointerData.position = Input.mousePosition;
		EventSystem.current.RaycastAll(pointerData, results);


		if(results.Count == 1) // drop over nothing
		{
			Return();
			EventDropOutside.Invoke(this);
			if (current) current.EventDropOutside.Invoke(this, current);
			return;
		}

		foreach(RaycastResult result in results)
		{
			DropArea a = result.gameObject.GetComponent<DropArea>();
			if(a)
			{
				if (autoMove)
				{
					if (!a.Drop(this, current))
						Return();
				}
				else Return();
				a.EventDropHere.Invoke(this, current, a);
				if(current) current.EventMoveOut.Invoke(this, current, a);
				return;
			}
		}

		// if we dont, return to where we came from
		Return();
	}

	/// <summary>
	/// This is the way to proceduraly move an item to another drop zone. forget all else.
	/// The code is rather spagehetti and I need to reconsider this.
	/// If I could use namespaces I could encapulate inside and outside the namespace to make all this much better
	/// </summary>
	/// <param name="destination"></param>
	public void DropIn(DropArea destination)
	{
		destination.Drop(this, current);
	}

	public void MoveTo(DropArea destination, Vector2 destinationLocalPosition)
	{
		current = destination;
		StopAllCoroutines();
		target.SetParent(destination.transform);
		StartCoroutine(CenterAt(destinationLocalPosition));
	}

	public void Return()
	{
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
		if (current) current.RemoveClient(this);
		current = null;
	}

	public class DropInAreaEvent : UnityEvent<Dropable, DropArea> { }
	public class DropableEvent : UnityEvent<Dropable> { }
}
