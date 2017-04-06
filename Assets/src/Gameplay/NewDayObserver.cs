using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class NewDayObserver : MonoBehaviour
{

	public UnityEvent EventNewDay = new UnityEvent();

	void OnEnable()
	{
		TimeAndDay.Instance.EventNewDay.AddListener(NewDay);
	}

	void OnDisable()
	{
		if (teardown) return;
		TimeAndDay.Instance.EventNewDay.RemoveListener(NewDay);
	}

	void NewDay()
	{
		EventNewDay.Invoke();
	}

	bool teardown = false;
	void OnApplicationQuit()
	{
		teardown = true;
	}
}
