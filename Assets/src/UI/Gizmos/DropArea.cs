﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// When a draggable is dropped over this, it fixates here
/// </summary>
public class DropArea : MonoBehaviour
{
	public int capacity = 1;
	public GameObject anchor;
	public int rows = 1;
	public Vector2 offset;

	List<Dropable> clients = new List<Dropable>();

	public DropEvent EventDropHere = new DropEvent();
	public DropEvent EventMoveOut = new DropEvent();

	void Start()
	{
		if (!anchor) anchor = gameObject;
	}
	
	/// <summary>
	/// Invoked when we want to move a dropable here.
	/// </summary>
	/// <param name="drop">the dropable dropped here</param>
	/// <param name="source">the source of drop</param>
	/// <returns></returns>
	virtual public bool Drop(Dropable drop, DropArea source)
	{
		if (source == this)	return false;
		if (clients.Count >= capacity) return false;
		if (clients.Contains(drop)) return false;

		if (source) source.RemoveClient(drop);

		
		clients.Add(drop);
		if (capacity > 1)
			ArrangeList();
		else
		{
			drop.Target.SetParent(anchor.transform);
			drop.transform.position = anchor.transform.position; // snap into position in single item lists
			drop.MoveTo(this, anchor.transform.localPosition);
		}
		return true;
	}

	public bool Contains(Dropable d)
	{
		return clients.Contains(d);
	}

	void MoveItemInList(Dropable client, DropArea other)
	{
		if (other == this) return;
		clients.Remove(client);
		client.EventDropInArea.RemoveListener(MoveItemInList);
		if (clients.Count > 0)
			ArrangeList();
	}

	void RemoveClient(Dropable d) // Do not make public!
	{
		clients.Remove(d);
		if (clients.Count > 0)
			ArrangeList();
	}

	void ArrangeList()
	{
		clients.Sort(SortFunction);
		for (int i = 0; i < clients.Count; i++)
		{
			clients[i].MoveTo(this, (Vector2)anchor.transform.localPosition + new Vector2(offset.x * (i / rows), offset.y * (i % rows)));
			// StartCoroutine(MoveTo(clients[i].transform, (Vector2)anchor.transform.position + new Vector2(offset.x * (i / rows), offset.y * (i % rows))));
		}
	}

	IEnumerator MoveTo(Transform target, Vector2 destination)
	{
		Vector2 start = target.position;
		for (float progress = 0; progress < 1f; progress += Time.deltaTime * 6f)
		{
			if (!target) break;
			target.position = Vector2.Lerp(start, destination, progress);
			yield return new WaitForSeconds(0f);
		}
		if(target)target.position = destination;
	}

	int SortFunction(Dropable a, Dropable b)
	{
		if (a.sortValue > b.sortValue) return -1;
		else if (b.sortValue > a.sortValue) return 1;
		return (int)(a.transform.position.x - b.transform.position.x - a.transform.position.y / 20 + b.transform.position.y / 20);
	}

	
	/// <summary>
	/// Dropable
	/// Source
	/// Destination
	/// </summary>
	[System.Serializable]
	public class DropEvent : UnityEvent<Dropable, DropArea, DropArea> { }
}
