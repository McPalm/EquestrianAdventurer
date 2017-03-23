using UnityEngine;
using System.Collections;
using System;


// script for an immobile Wall
[RequireComponent(typeof(BoxCollider2D))]
public class Wall : MonoBehaviour, IMapBlock
{
	public bool SeeThrough = false;

	public bool BlockMove
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

	void OnDisable()
	{
		BlockMap.Instance.Remove(gameObject);
	}

	void OnEnable()
	{
		BlockMap.Instance.Add(this, gameObject);
		gameObject.layer = LayerMask.NameToLayer("Wall");
	}
}
