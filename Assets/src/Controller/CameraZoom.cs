using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraZoom : MonoBehaviour {

    public float maxSize = 10f;
    public float minSize = 4f;
	

	// Update is called once per frame
	void Update ()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;
		float f = Input.GetAxis("Mouse ScrollWheel");
		if(f < -0f)
		{
			Camera.main.orthographicSize *= 1.34f;
            if (Camera.main.orthographicSize > maxSize) Camera.main.orthographicSize = maxSize;

        }
		else if(f > 0f)
		{
			Camera.main.orthographicSize /= 1.34f;
            if (Camera.main.orthographicSize < minSize) Camera.main.orthographicSize = minSize;
        }
	}
}
