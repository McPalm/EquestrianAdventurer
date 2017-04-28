using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GroupModulesUI : MonoBehaviour
{
	[SerializeField]
	GameObject prefab;
	[SerializeField]
	Button addButton;
	[SerializeField]
	float padding;

	List<GameObject> thingies = new List<GameObject>();
	List<string> values = new List<string>();

	public StringArrayEvent EventChange = new StringArrayEvent();

	public void Start()
	{
		addButton.onClick.AddListener(Add);
	}

	public void Build(string[] items)
	{
		values = new List<string>(items);

		int count = (items.Length < thingies.Count) ? thingies.Count : items.Length;
		for(int i = 0; i < count; i++)
		{
			if(i < items.Length)
			{
				if(i == thingies.Count)
				{
					thingies.Add(Instantiate(prefab));
					thingies[i].transform.SetParent(transform);
					thingies[i].transform.localPosition = new Vector3(0f, 0f);
					thingies[i].GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, i * 30f + padding, 30f);

					int capture = i;
					UnityAction delete = () =>
					{
						Remove(capture);
					};
					UnityAction<string> write = (string s) =>
					{
						WriteTo(capture, s);
					};

					thingies[i].GetComponentInChildren<Button>().onClick.AddListener(delete);
					thingies[i].GetComponentInChildren<InputField>().onEndEdit.AddListener(write);
					
				}

				thingies[i].SetActive(true);
				thingies[i].GetComponentInChildren<InputField>().text = items[i];
			}
			else
			{
				thingies[i].SetActive(false);
			}
			
		}
		GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, items.Length * 30f + 30f + padding * 2f);
	}

	public void Add()
	{
		values.Add("");
		EventChange.Invoke(values.ToArray());
	}

	public void Remove(int i)
	{
		values.RemoveAt(i);
		EventChange.Invoke(values.ToArray());
	}

	public void WriteTo(int i, string content)
	{
		values[i] = content;
		EventChange.Invoke(values.ToArray());
	}

	[System.Serializable]
	public class StringArrayEvent : UnityEvent<string[]> { }
}
