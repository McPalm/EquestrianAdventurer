using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Interactable))]
public class Door : MonoBehaviour, IMapBlock
{
	public bool closed = true;

	public GameObject OpenIcon;
	public GameObject ClosedIcon;

	public bool BlockMove
	{
		get
		{
			return closed;
		}
	}

	public bool BlockSight
	{
		get
		{
			return closed;
		}
	}

	public bool Interactable
	{
		get
		{
			return true;
		}
	}

	public Interactable MyInteractable
	{
		get
		{
			return GetComponent<Interactable>();
		}
	}

	// Use this for initialization
	void Start()
	{
		GetComponent<Interactable>().Radius = 1;
		GetComponent<Interactable>().EventInteract.AddListener(OpenClose);
		OpenIcon.gameObject.SetActive(!closed);
		ClosedIcon.gameObject.SetActive(closed);
	}

	void OpenClose(MapObject user)
	{
		closed = !closed;
		OpenIcon.gameObject.SetActive(!closed);
		ClosedIcon.gameObject.SetActive(closed);
	}

	void OnApplicationQuit()
	{
		teardown = true;
	}

	bool teardown = false;
	void OnDisable()
	{
		if (teardown) return;
		if (MapBuildController.editing) return;
		BlockMap.Instance.Remove(gameObject);
	}

	void OnEnable()
	{
		if (MapBuildController.editing) return;
		BlockMap.Instance.Add(this, gameObject);
		gameObject.layer = LayerMask.NameToLayer("Wall");

	}
}
