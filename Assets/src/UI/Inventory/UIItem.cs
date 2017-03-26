using UnityEngine;
using System.Collections;

public class UIItem : Dropable
{
	Item item;

	public Item Item
	{
		get
		{
			return item;
		}

		set
		{
			item = value;
		}
	}

	new protected void Start()
	{
		target = transform;

		base.Start();

		Equipment mock = new Equipment();

		mock.displayName = "armor";
		mock.armor = 5;
		mock.slots = EquipmentType.body;

		item = mock;

	}

}
