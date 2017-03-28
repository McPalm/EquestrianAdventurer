using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CombatTextPool : MonoBehaviour
{

	static CombatTextPool _instance;

	static public CombatTextPool Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<CombatTextPool>();
			return _instance;
		}
	}

	public Text prefab;

	List<Text> texts = new List<Text>();
	int count = 0;

	// Use this for initialization
	void Start ()
	{
		prefab.gameObject.SetActive(false);
		for(int i = 0; i < 20; i++)
		{
			texts.Add(Instantiate(prefab));
			texts[i].transform.SetParent(transform);
		}
	}
	
	public void PrintAt(Vector2 location, string text, Color color, float duration = 0.75f)
	{
		texts[count].text = text;
		texts[count].color = color;
		texts[count].gameObject.SetActive(true);
		texts[count].transform.position = Camera.main.WorldToScreenPoint(location);


		StartCoroutine(HideAfterSeconds(texts[count], duration));
		count++;
		count %= texts.Count;
	}

	IEnumerator HideAfterSeconds(Text text, float time)
	{
		yield return new WaitForSeconds(time);
		text.gameObject.SetActive(false);
	}
}
