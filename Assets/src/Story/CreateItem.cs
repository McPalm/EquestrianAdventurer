using UnityEngine;
using System.Collections.Generic;

public class CreateItem : MonoBehaviour
{

	static CreateItem _instance;

	public static CreateItem Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = Resources.Load<CreateItem>("ItemDB");
				_instance.Init();
			}
			return _instance;
		}
	}

	[SerializeField]
	Container[] items;

	Dictionary<string, GroundItem> dict = new Dictionary<string, GroundItem>();


	// Use this for initialization
	void Init()
	{
		for(int i = 0; i < items.Length; i++)
		{
			dict.Add(items[i].tag, items[i].item);
		}
	}

	public bool TryGet(string tag, out Item i)
	{
		i = null;
		GroundItem g = null;
		if(dict.TryGetValue(tag, out g))
		{
			i = g.CloneItem();
			return true;
		}
		return false;
	}
	
	[System.Serializable]
	public class Container
	{
		public string tag;
		public GroundItem item;
	}
}
