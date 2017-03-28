using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class OOBTint : MonoBehaviour
{
	public Color inside;
	public Color outside;
	public MapSection target;
	SpriteRenderer sr;

	// Use this for initialization
	void Start ()
	{
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target)
		{
			if (target.IsInSection(transform.position))
			{
				sr.color = inside;
			}
			else sr.color = outside;
		}
		else
			sr.color = outside;
	}
}
