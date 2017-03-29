using UnityEngine;
using System.Collections.Generic;

public class UIItemPool : MonoBehaviour
{
	public UIItem prefab;

	static UIItemPool _instance;

	public static UIItemPool Instance
	{
		get
		{
			return _instance;
		}
	}

	// List<UIItem> active = new List<UIItem>();
	Dictionary<Item, UIItem> active = new Dictionary<Item, UIItem>();

	void Awake()
	{
		_instance = this;
	}

	public UIItem Get(Item i)
	{
		UIItem r = null;
		if (active.TryGetValue(i, out r))
			return r;
		return Build(i);
	}

	UIItem Build(Item i)
	{
		UIItem ret = Instantiate(prefab);
		active.Add(i, ret);
		ret.transform.SetParent(transform);
		ret.Item = i;
		ret.transform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // we appear in the middle of the screen. Improve this later maybe
		return ret;
	}
}
