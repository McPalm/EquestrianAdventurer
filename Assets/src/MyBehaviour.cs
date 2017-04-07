using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Safe teardown for things that need to subscribe/unsubscribe
/// </summary>
public class MyBehaviour : MonoBehaviour
{
	protected UnityEvent EventDisable = new UnityEvent();
	protected UnityEvent EventDestroy = new UnityEvent();
	protected UnityEvent EventQuit = new UnityEvent();

	bool teardown = false;

	protected void OnDisable()
	{
		if (teardown) return;
		EventDisable.Invoke();
	}

	protected void OnDestroy()
	{
		if (teardown) return;
		EventDestroy.Invoke();
	}

	protected void OnApplicationQuit()
	{
		teardown = true;
		EventQuit.Invoke();
	}
}
