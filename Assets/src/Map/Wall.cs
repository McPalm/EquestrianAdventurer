using UnityEngine;
using System.Collections;
using System;


// script for an immobile Wall
[RequireComponent(typeof(BoxCollider2D))]
public class Wall : MonoBehaviour, IMapBlock
{
	public bool SeeThrough = false;
	Interactable interactable;


	virtual public bool BlockMove
	{
		get
		{
			return true;
		}
	}

	public bool BlockSight
	{
		get
		{
			return !SeeThrough;
		}
	}

	public bool Interactable
	{
		get
		{
			return interactable;
		}
	}

	public Interactable MyInteractable
	{
		get
		{
			return interactable;
		}
	}

	void OnApplicationQuit()
	{
		on = false;
	}

	bool on = true;

	void OnDisable()
	{
		if (MapBuildController.editing) return;
		if (on)BlockMap.Instance.Remove(gameObject);
	}

	void OnEnable()
	{
		if (MapBuildController.editing) return;
		BlockMap.Instance.Add(this, gameObject);
		gameObject.layer = LayerMask.NameToLayer("Wall");
	}

	void Start()
	{
		interactable = GetComponent<Interactable>();
	}
}
