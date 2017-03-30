using UnityEngine;
using System.Collections;

public class TileVisbility : MonoBehaviour
{
	public bool BlockSight;

	bool visible;

	public bool Visible
	{
		get
		{
			return visible;
		}
	}

	void OnEnable()
	{
		Hide();
		if (GetComponent<Wall>())
		{
			BlockSight = GetComponent<Wall>().BlockSight;
		}
		SightRadius.Instance.AddVisbility(this, IntVector2.RoundFrom(transform.position));
	}

	void OnApplicationQuit()
	{
		teardown = true;
	}
	bool teardown = false;
	void OnDisable()
	{
		if (teardown) return;
		SightRadius.Instance.RemoveTile(IntVector2.RoundFrom(transform.position));
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	public void Show()
	{
		visible = true;
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>(true))
			sr.enabled = true;
	}

	public void Hide()
	{
		visible = false;
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
			sr.enabled = false;
	}
}
