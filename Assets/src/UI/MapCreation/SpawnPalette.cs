using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class SpawnPalette : MonoBehaviour
{
	[SerializeField]
	GameObject[] prefabs;
	[SerializeField]
	GameObject swatch1;
	[SerializeField]
	Sprite fallbackSprite;

	List<GameObject> Swatches = new List<GameObject>();

	public GameObjectEvent EventPlace = new GameObjectEvent();

	void Start()
	{
		swatch1.SetActive(false);
		Build();
	}

	public void Build()
	{
		int cutoff = Mathf.Max(prefabs.Length, Swatches.Count);
		for(int i = 0; i < cutoff; i++)
		{
			if(i == Swatches.Count)
			{
				// make new swatch
				Swatches.Add(Instantiate(swatch1));
				Swatches[i].transform.position = swatch1.transform.position + new Vector3(34f, 0f) * (i % 10) + new Vector3(0f, -34f) * (i / 10);
				Swatches[i].transform.SetParent(swatch1.transform.parent);
				int capture = i;
				UnityAction<GameObject> placeDel = (o) =>
				{
					Place(capture);
				};
				Swatches[i].GetComponentInChildren<Draggable>(true).EventStopDrag.AddListener(placeDel);
			}
			if(i < prefabs.Length)
			{
				// assign swatch properly
				Swatches[i].SetActive(true);
				if (prefabs[i].GetComponent<SpriteRenderer>())
					Swatches[i].GetComponentInChildren<Draggable>().GetComponent<Image>().sprite = prefabs[i].GetComponent<SpriteRenderer>().sprite;
				else
					Swatches[i].GetComponentInChildren<Draggable>().GetComponent<Image>().sprite = fallbackSprite;
			}
			else
			{
				// hide swatch
				Swatches[i].SetActive(false);
			}
		}
	}

	void Place(int i)
	{
		EventPlace.Invoke(prefabs[i]);
	}

	[System.Serializable]
	public class GameObjectEvent : UnityEvent<GameObject> { }
}
