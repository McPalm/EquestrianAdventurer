using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UIInventory : MonoBehaviour
{
	public DropArea Equipment;

	public UIItem UIItemPrefab;

	public UIEquipmentSlot WeaponSlot;
	public UIEquipmentSlot ArmorSlot;

	public Inventory target;

	void Start()
	{
		target.EventAddItem.AddListener(ModelAddItem);
	}

	/// <summary>
	/// Whenever the Inventory gets a new item added.
	/// Such as picking up from the ground
	/// </summary>
	/// <param name="i"></param>
	void ModelAddItem(Item i)
	{
		print("Adding "+ i.displayName);
		Equipment.Drop(Build(i));
	}


	UIItem Build(Item i)
	{
		UIItem ret = Instantiate(UIItemPrefab);
		ret.transform.SetParent(transform);
		ret.Item = i;
		ret.transform.position = Camera.main.WorldToScreenPoint(target.transform.position); // make the item over the player. Should add a cool effect of it going from the ground to the inventory
		return ret;
	}

	public class ItemEvent : UnityEvent<Item> { }
}
