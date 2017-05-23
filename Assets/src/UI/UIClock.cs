using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIClock : MonoBehaviour
{
	[SerializeField]
	DiscMeter hourHand;
	[SerializeField]
	DiscMeter minuteHand;
	[SerializeField]
	Text digital;
	[SerializeField]
	Text AMPM;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Tick());
	}
	
	
	IEnumerator Tick()
	{
		TimeAndDay t = TimeAndDay.Instance;
		while (Application.isPlaying)
		{
			float hour = t.Hour + t.Minute / 60f;
			hour /= 12f;
			hourHand.Value = hour;

			minuteHand.Value = t.Minute / 60f;

			int dhour = t.Hour % 12;
			if (dhour == 0) dhour = 12;
			digital.text = dhour + ((t.Minute < 10) ? ":0" : ":") + t.Minute;

			AMPM.text = (t.Hour < 12) ? "AM" : "PM";

			yield return new WaitForSeconds(0.1f);
		}
	}
}
