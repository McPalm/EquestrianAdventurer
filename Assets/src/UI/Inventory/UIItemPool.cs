using UnityEngine;
using System.Collections.Generic;

public class UIItemPool : MonoBehaviour
{
	static UIItemPool _instance;
	public static UIItemPool Instance
	{
		get
		{
			return _instance;
		}	
	}

	public UIItem prefab;

	Dictionary<Item, UIItem> active = new Dictionary<Item, UIItem>();
	public Stack<UIItem> inactive = new Stack<UIItem>();

	void Awake()
	{
		_instance = this;
	}

	/// <summary>
	/// Get the UIItem containing the given item
	/// </summary>
	/// <param name="i">the item</param>
	/// <param name="b">true the item is new</param>
	/// <returns></returns>
	public bool Get(Item i, out UIItem uitem)
	{
		if (active.TryGetValue(i, out uitem))
			return false;
		uitem = Build(i);
		return true;
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
		UIItem ret;
		if (inactive.Count == 0) ret = Instantiate(prefab);
		else
		{
			ret = inactive.Pop();
			ret.gameObject.SetActive(true);
		}
		active.Add(i, ret);
		// ret.transform.SetParent(transform);
		ret.Item = i;
		ret.transform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // we appear in the middle of the screen. Improve this later maybe
		return ret;
	}

	/// <summary>
	/// Call this when we no longer need an UI item
	/// </summary>
	/// <param name="i"></param>
	public void Deactivate(Item i)
	{
		UIItem u = Get(i);
		active.Remove(i);
		inactive.Push(u);
		u.gameObject.SetActive(false);
	}
}
