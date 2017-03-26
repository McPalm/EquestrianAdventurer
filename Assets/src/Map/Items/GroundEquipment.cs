using UnityEngine;
using System.Collections;

/// <summary>
/// For use in editor only
/// </summary>
public class GroundEquipment : GroundItem
{
	public Equipment equipment;

	new void Start()
	{
		item = equipment;
		base.Start();	
	}
}
