using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TimeAndDay : MonoBehaviour
{
	static TimeAndDay _instance;

	public static TimeAndDay Instance
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType<TimeAndDay>();
			return _instance;
		}
	}

	public UnityEvent EventNewDay = new UnityEvent();

	public void NewDay()
	{
		EventNewDay.Invoke();
	}
}
