using UnityEngine;
using System.Collections;

public class TileSnap : MonoBehaviour
{
	
	// Update is called once per frame
	void OnDrawGizmosSelected()
	{
		if (Application.isPlaying) return;
		transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y), Mathf.Round(transform.localPosition.z));
	}
}
