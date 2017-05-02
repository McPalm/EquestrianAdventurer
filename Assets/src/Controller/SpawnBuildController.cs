using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SpawnBuildController : MonoBehaviour
{
	[SerializeField]
	GameObject[] tools;
	[SerializeField]
	PremadeSpawner spawner;


	System.Action mode;
	HashSet<SpriteRenderer> placed = new HashSet<SpriteRenderer>();
	SpriteRenderer pickMeUp;

	void Start()
	{
		if (spawner)
			LoadSpawner();
	}

	void OnEnable()
	{
		// show stuffs
		foreach (GameObject o in tools) o.SetActive(true);
		mode = Neutral;
	}

	void OnDisable()
	{
		// hide stuffs
		foreach (GameObject o in tools) o.SetActive(false);
	}

	void Update()
	{
		mode();
	}

	void Neutral()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

		if(Input.GetMouseButtonDown(0))
		{
			//placed.TryGetValue(IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition)), out pickMeUp);
			pickMeUp = GetAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (pickMeUp)
				mode = Hold;
		}
	}

	void Hold()
	{
		pickMeUp.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButtonUp(0))
		{
			print("Drop!");
			mode = Neutral;
			pickMeUp.transform.position = (Vector2)IntVector2.RoundFrom(pickMeUp.transform.position);
			pickMeUp = null;
		}
		else if(Input.GetKeyDown(KeyCode.Escape))
		{
			placed.Remove(pickMeUp);
			Destroy(pickMeUp.gameObject);
			pickMeUp = null;
			mode = Neutral;
		}
	}

	SpriteRenderer GetAt(Vector2 v2)
	{
		foreach(SpriteRenderer sr in placed)
		{
			if (((Vector2)sr.transform.position - v2).magnitude < 0.5f)
				return sr;
		}
		return null;
	}

	public void PlaceAt(IntVector2 iv2, GameObject o)
	{
		GameObject go = new GameObject(o.name);
		go.AddComponent<SortRenderingOrder>();
		SpriteRenderer r = go.AddComponent<SpriteRenderer>();
		r.sortingLayerName = "Active";
		placed.Add(r);
		r.sprite = o.GetComponent<SpriteRenderer>().sprite;
		go.transform.position = (Vector3)iv2;
		go.AddComponent<Reference>().prefab = o;
	}
		 
	public void Place(GameObject o) // this is going to need improved
	{
		PlaceAt(IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition)), o);
	}

	public void LoadSpawner()
	{
		foreach(PremadeSpawner.SpawnContainer c in spawner.spawnlist)
		{
			PlaceAt(c.offset, c.target);
		}
	}

	public void Save()
	{
		List<PremadeSpawner.SpawnContainer> list = new List<PremadeSpawner.SpawnContainer>();
		foreach(SpriteRenderer o in placed)
		{
			PremadeSpawner.SpawnContainer container;
			container.target = o.GetComponent<Reference>().prefab;
			container.offset = IntVector2.RoundFrom(o.transform.position);
			list.Add(container);
		}
		spawner.spawnlist = list.ToArray();
	}


	class Reference : MonoBehaviour
	{
		public GameObject prefab;
	}
}

