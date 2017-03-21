using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour
{

	IntVector2 realLocation;

	public IntVector2 RealLocation
	{
		get
		{
			return realLocation;
		}

		set
		{
			realLocation = value;
		}
	}

	public void OnEnable()
	{
		realLocation = IntVector2.RoundFrom(transform.position);
	}
}
