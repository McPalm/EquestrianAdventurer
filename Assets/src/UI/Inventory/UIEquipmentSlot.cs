using UnityEngine;
using System.Collections;

public class UIEquipmentSlot : DropArea
{
	public EquipmentType slotType;

	override public bool Drop(Dropable d)
	{
		UIItem i = d.GetComponent<UIItem>();
		if (!i) return false;

		if (((i.Item as Equipment).slots & slotType) == 0)
			return false;

		return base.Drop(d);
	}
}
