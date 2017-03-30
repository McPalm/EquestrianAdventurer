using UnityEngine;
using System.Collections;

public class KillAfterSeconds : MonoBehaviour {

	public float seconds = 1f;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(KillAfter(seconds));
	}
	
	IEnumerator KillAfter(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}
