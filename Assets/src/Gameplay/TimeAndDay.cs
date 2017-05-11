using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TimeAndDay : MonoBehaviour, TurnTracker.TurnEntry
{
	static TimeAndDay _instance;

	int day = 1;
	int hour = 10;
	int second;

	const int SecPerHour = 400;
	const int HourPerDay = 24;

	public static TimeAndDay Instance
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType<TimeAndDay>();
			return _instance;
		}
	}

	public UnityEvent EventNewDay = new UnityEvent();
	public TimeEvent EventNewHour = new TimeEvent();

	void Start()
	{
		TurnTracker.Instance.Add(this);
	}

	public void NewDay()
	{
		day++;
		hour = 10;
		second = 0;
		EventNewDay.Invoke();
	}

	public void DoTurn()
	{
		second++;
		if(second == SecPerHour)
		{
			second = 0;
			hour++;
			if(hour == HourPerDay)
			{				
				NewDay();
			}
			EventNewHour.Invoke(hour);
		}
	}

	[System.Serializable]
	public class TimeEvent : UnityEvent<int> { }
}
