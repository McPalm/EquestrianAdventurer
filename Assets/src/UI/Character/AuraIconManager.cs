using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuraIconManager : MonoBehaviour
{
	static AuraIconManager _instance;
	public AuraIcon prefab;
	public float distance;

	Stack<AuraIcon> inactive = new Stack<AuraIcon>();
	List<AuraIcon> active = new List<AuraIcon>();

	public static AuraIconManager Instance
	{
		get
		{
			return _instance;
		}
	}

	// Use this for initialization
	void Awake()
	{
		_instance = this;
	}
	
	public void Add(Aura a)
	{
		AuraIcon i;
		if (inactive.Count == 0)
		{
			i = Instantiate(prefab);
			i.transform.SetParent(transform);
		}
		else
			i = inactive.Pop();
		i.gameObject.SetActive(true);
		i.SetTarget(a);
		i.transform.localPosition = new Vector3(active.Count * distance, 0f, 0f);
		active.Add(i);
	}

	public void Deactivate(AuraIcon i)
	{
		active.Remove(i);
		inactive.Push(i);
		Shuffle();
		i.gameObject.SetActive(false);
	}

	public void Shuffle()
	{
		StopAllCoroutines();
		for(int i = 0; i < active.Count; i++)
		{
			StartCoroutine(Shuffle(active[i].transform, new Vector3(i * distance, 0f), 0.25f));
		}
	}

	IEnumerator Shuffle(Transform t, Vector3 destination, float speed)
	{
		Vector3 start = t.localPosition;
		for(float f = 0; f < speed; f += Time.deltaTime)
		{
			t.localPosition = Vector3.Lerp(start, destination, f / speed);
			yield return new WaitForSeconds(0f);
		}
		t.localPosition = destination;
	}
}
