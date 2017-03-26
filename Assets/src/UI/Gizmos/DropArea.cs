using UnityEngine;
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

	void Start()
	{
		if (!anchor) anchor = gameObject;
	}
	
	public bool Drop(Dropable drop)
	{
		if (clients.Count >= capacity) return false;
		if (clients.Contains(drop)) return false;

		clients.Add(drop);
		drop.EventDropInArea.AddListener(MoveItemInList);

		if (capacity > 1)
			ArrangeList();
		else
			drop.transform.position = anchor.transform.position;
		return true;
	}

	void MoveItemInList(Dropable client, DropArea other)
	{
		if (other == this) return;
		clients.Remove(client);
		client.EventDropInArea.RemoveListener(MoveItemInList);
		if (clients.Count > 0)
			ArrangeList();
	}

	void ArrangeList()
	{
		clients.Sort(SortFunction);

		for(int i = 0; i < clients.Count; i++)
		{
			StartCoroutine(MoveTo(clients[i].transform, (Vector2)anchor.transform.position + new Vector2(offset.x * (i / rows), offset.y * (i % rows))));
		}
	}

	IEnumerator MoveTo(Transform target, Vector2 destination)
	{
		Vector2 start = target.position;
		for (float progress = 0; progress < 1f; progress += Time.deltaTime * 6f)
		{
			target.position = Vector2.Lerp(start, destination, progress);
			yield return new WaitForSeconds(0f);
		}
		target.position = destination;
	}

	int SortFunction(Dropable a, Dropable b)
	{
		if (a.sortValue > b.sortValue) return -1;
		else if (b.sortValue > a.sortValue) return 1;
		return 0;
	}

}
