using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CameraDrag : MonoBehaviour {

	static CameraDrag _instance;

	Action State;

	Vector3 clickPosition, cameraLocationOnClick;

	public UnityEvent EventStartDrag;
	public UnityEvent EventStopDrag;

	public static CameraDrag Instance 	{ get { return _instance; } }


	void Awake()
	{
		State = Neutral;
		_instance = this;
	}

	// Update is called once per frame
	void Update ()
	{
		State();
	}

	void Neutral()
	{
		if (Input.GetMouseButtonDown(1))
		{
			if (EventSystem.current.IsPointerOverGameObject() == false) // not clicking on UI
			{
				State = MouseDown;
				clickPosition = Input.mousePosition;
				cameraLocationOnClick = transform.position;
			}
		}
	}

	void MouseDown()
	{
		Vector3 dragdelta = Input.mousePosition - clickPosition;
		
		if (Input.GetMouseButtonUp(1)) State = Neutral;
		else if (dragdelta.x > 29f || dragdelta.y > 29f || dragdelta.x < -29f || dragdelta.y < -29f)
		{
			State = Dragging;
			EventStartDrag.Invoke();
		}
		
	}

	void Dragging()
	{
		transform.position = cameraLocationOnClick + (clickPosition - Input.mousePosition) * Camera.main.orthographicSize / Camera.main.pixelHeight * 2f;
		if (Input.GetMouseButtonUp(1)) State = TransitionToNeutral;
	}

	void TransitionToNeutral()
	{
		EventStopDrag.Invoke();
		State = Neutral;
	}
}
