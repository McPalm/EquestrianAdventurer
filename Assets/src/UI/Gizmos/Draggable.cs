using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	protected Transform target;
	private bool isMouseDown = false;
	private Vector3 startMousePosition;
	private Vector3 startPosition;
	public bool shouldReturn;

	public DragEvent EventStartDrag;
	public DragEvent EventStopDrag;

	public Transform Target
	{
		get
		{
			return target;
		}
	}

	public void Awake()
	{
		if (EventStartDrag == null) EventStartDrag = new DragEvent();
		if (EventStopDrag == null) EventStopDrag = new DragEvent();
	}

	public void OnPointerDown(PointerEventData dt)
	{
		isMouseDown = true;
		EventStartDrag.Invoke(gameObject);

		startPosition = target.position;
		startMousePosition = Input.mousePosition;
	}

	public void OnPointerUp(PointerEventData dt)
	{
		isMouseDown = false;
		EventStopDrag.Invoke(gameObject);

		if (shouldReturn)
		{
			target.position = startPosition;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isMouseDown)
		{
			Vector3 currentPosition = Input.mousePosition;

			Vector3 diff = currentPosition - startMousePosition;

			Vector3 pos = startPosition + diff;

			target.position = pos;
		}
	}

	[System.Serializable]
	public class DragEvent : UnityEvent<GameObject> { }
}