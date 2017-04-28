using UnityEngine;
using System.Collections;

public class SectionMapIcon : MonoBehaviour
{
	[SerializeField]
	GameObject FocusTileMarker;
	[SerializeField]
	GameObject FocusGroupMarker;
	[SerializeField]
	GameObject ModuleMarker;
	[SerializeField]
	GameObject CustomMarker;

	[Space(10f)]

	[SerializeField]
	GameObject NorthConnection;
	[SerializeField]
	GameObject EastConnection;
	[SerializeField]
	GameObject SouthConnection;
	[SerializeField]
	GameObject WestConnection;
	
	
	public Color Color
	{
		set
		{
			GetComponent<SpriteRenderer>().color = value;
		}
	}

	public bool FocusTile
	{
		set
		{
			FocusTileMarker.gameObject.SetActive(value);
		}
	}

	public bool FocusGroup
	{
		set
		{
			FocusGroupMarker.gameObject.SetActive(value);
		}
	}

	public CompassDirection SetConnections
	{
		set
		{
			NorthConnection.gameObject.SetActive((value & CompassDirection.north) == CompassDirection.north);
			EastConnection.gameObject.SetActive((value & CompassDirection.east) == CompassDirection.east);
			SouthConnection.gameObject.SetActive((value & CompassDirection.south) == CompassDirection.south);
			WestConnection.gameObject.SetActive((value & CompassDirection.west) == CompassDirection.west);
		}
	}

	public bool Module
	{
		set
		{
			ModuleMarker.SetActive(value);
		}
	}

	public bool Custom
	{
		set
		{
			CustomMarker.SetActive(value);
		}
	}
}
