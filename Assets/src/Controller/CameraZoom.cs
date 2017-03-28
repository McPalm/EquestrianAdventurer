using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraZoom : MonoBehaviour {

	
	

	// Update is called once per frame
	void Update ()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;
		float f = Input.GetAxis("Mouse ScrollWheel");
		if(f < -0f)
		{
			Camera.main.orthographicSize *= 1.34f;
		}
		else if(f > 0f)
		{
			Camera.main.orthographicSize /= 1.34f;
		}
	}
}
