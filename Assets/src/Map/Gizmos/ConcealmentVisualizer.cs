using UnityEngine;
using System.Collections;

public class ConcealmentVisualizer : MonoBehaviour
{
	SpriteRenderer sr;
	MapObject me;

	public Color hideColor;

	// Use this for initialization
	void Start ()
	{
		sr = GetComponent<SpriteRenderer>();
		me = GetComponent<MapObject>();
	}
	
	public void Refresh()
	{
		if (me.HasConcealment)
			sr.color = hideColor;
		else
			sr.color = Color.white;
	}
}
