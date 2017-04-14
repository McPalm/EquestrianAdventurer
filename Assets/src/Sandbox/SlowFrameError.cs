using UnityEngine;
using System.Collections;

public class SlowFrameError : MonoBehaviour
{

	
	// Update is called once per frame
	void Update () {
		if (Time.deltaTime > 1f)
			Debug.LogError("Lagspike, frame took " + Time.deltaTime + " to process.");
	}
}
