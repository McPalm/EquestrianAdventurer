using UnityEngine;
using System.Collections;

public class DiscMeter : MonoBehaviour
{
	public bool clockWise = true;

	public float Value
	{
		set
		{
			transform.rotation = Quaternion.AngleAxis((clockWise) ? value * -360f : value * 360f, Vector3.forward);
		}
	}
}