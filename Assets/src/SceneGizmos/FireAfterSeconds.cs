using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FireAfterSeconds : MonoBehaviour
{
	public UnityEvent Event = new UnityEvent();


	public void FireAfter(float seconds)
	{
		StartCoroutine(Delay(seconds));
	}

	public void Clear()
	{
		StopAllCoroutines();
	}

	IEnumerator Delay(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Event.Invoke();
	}
}
