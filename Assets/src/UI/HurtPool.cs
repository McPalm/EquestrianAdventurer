using UnityEngine;
using System.Collections.Generic;

public class HurtPool : MonoBehaviour
{

	static HurtPool _instance;

	public DamageBox prefab;

	List<DamageBox> list = new List<DamageBox>();

	int count;

	public static HurtPool Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<HurtPool>();
			return _instance;
		}
	}

	// Use this for initialization
	void Start ()
	{
		prefab.gameObject.SetActive(false);
		for(int i = 0; i < 20; i++)
		{
			list.Add(Instantiate(prefab));
			list[i].transform.SetParent(transform);
		}
		list.Add(prefab);
	}
	
	public void DoHurt(IntVector2 location, int damage)
	{
		list[count].Display((Vector3)location, damage);
		count++;
		count %= list.Count;
	}
}
