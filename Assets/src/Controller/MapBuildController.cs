using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class MapBuildController : MonoBehaviour
{
	static public bool editing = false;

	public string palette;
	private TileDB tileDB;

	[SerializeField]
	SpriteRenderer marker;
	[SerializeField]
	SpriteRenderer MarkerPrefab;
	List<SpriteRenderer> rectMarker = new List<SpriteRenderer>();

	public MapSection editSection;
	Sprite s;
	Sprite b;
	int swatch;

	Action State;
	Vector2 drawStart;
	Vector2 lastPos;

	public bool hollowRect = true;

	protected void OnEnable()
	{
		tileDB = TileDB.LoadPalette(palette);
		marker.gameObject.SetActive(true);
		marker.GetComponent<OOBTint>().target = editSection;
		b = marker.sprite;
		State = DrawRectPassive;
		editing = true;
	}

	protected void OnDisable()
	{
		editing = false;
	}

	public void SetSwatch(int i)
	{
		Color c = Color.white;
		// bool flip = false;
		if (i < 0) s = b;
		else
			s = tileDB.GetSprite(i, out c);  //tiles[i].GetComponent<SpriteRenderer>().sprite;  // I dont actually know what this does
		// else s = TileDB.GetTile(i, out flip);
		swatch = i;
		marker.GetComponent<SpriteRenderer>().sprite = s;
		marker.GetComponent<SpriteRenderer>().color = c;
		marker.GetComponent<OOBTint>().inside = c;
		//marker.GetComponent<SpriteRenderer>().flipX = flip;
	}

	public void Update()
	{
		State();
	}

	public void Draw()
	{
		if (Blocked) return;

		if(Input.GetMouseButton(0))
		{
			if (editSection.IsInSection(marker.transform.position))
			{
				editSection.SetTile(marker.transform.position, swatch);
			}
		}
	}

	void Fill()
	{
		if (Blocked) return;

		if (Input.GetMouseButtonDown(0))
		{
			if (editSection.IsInSection(marker.transform.position))
			{
				editSection.Fill(marker.transform.position, swatch);
			}
		}
	}

	void DrawRectPassive()
	{
		if (Blocked) return;

		if (Input.GetMouseButtonDown(0))
		{
			if (editSection.IsInSection(marker.transform.position))
			{
				drawStart = marker.transform.position;
				State = DrawRectActive;
				lastPos = new Vector2(0.2f, 999999999.9f);
			}
		}
	}

	void DrawRectActive()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			State = DrawRectPassive;
			foreach (SpriteRenderer sr in rectMarker) sr.gameObject.SetActive(false);
		}

		if (Blocked) return;
		if (editSection.IsInSection(marker.transform.position))
		{
			if (lastPos != (Vector2) marker.transform.position)
			{
				lastPos = marker.transform.position;
				DrawRect(marker.transform.position, drawStart);
			}
			if (Input.GetMouseButtonDown(0))
			{
				for(int i = 0; i < rectMarker.Count; i++)
				{
					if(rectMarker[i].gameObject.activeSelf)
						editSection.SetTile(rectMarker[i].transform.position, swatch);
				}
				State = DrawRectPassive;
				foreach (SpriteRenderer sr in rectMarker) sr.gameObject.SetActive(false);
			}
		}
	}

	public void DrawRect(Vector2 a, Vector2 b)
	{
		int sx = Mathf.RoundToInt(Mathf.Min(a.x, b.x));
		int sy = Mathf.RoundToInt(Mathf.Min(a.y, b.y));
		int ex = Mathf.RoundToInt(Mathf.Max(a.x, b.x)) + 1;
		int ey = Mathf.RoundToInt(Mathf.Max(a.y, b.y)) + 1;
		int i = 0;
		for(int x = sx; x < ex; x++)
		{
			for(int y = sy; y < ey; y++)
			{
				if (hollowRect)
					if ((x == sx || x == ex - 1 || y == sy || y == ey - 1) == false)
						continue;

				if(i == rectMarker.Count)
				{
					rectMarker.Add(Instantiate(MarkerPrefab));
					rectMarker[i].sprite = this.b;
				}
				rectMarker[i].transform.position = new Vector3(x, y);
				rectMarker[i].gameObject.SetActive(true);
				i++;
				
			}
		}
		while (i < rectMarker.Count)
		{
			rectMarker[i].gameObject.SetActive(false);
			i++;
		}
	}

	/// <summary>
	/// 1 = draw, 2 = fill, 3 = filledRect, 4 = emptyRect
	/// </summary>
	/// <param name="i"></param>
	public void EnterMode(int i)
	{
		switch(i)
		{
			case 1:
				State = Draw;
				break;
			case 2:
				State = Fill;
				break;
			case 3:
				State = DrawRectPassive;
				hollowRect = false;
				break;
			case 4:
				State = DrawRectPassive;
				hollowRect = true;
				break;
		}
	}

	bool Blocked
	{
		get
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
		
}


