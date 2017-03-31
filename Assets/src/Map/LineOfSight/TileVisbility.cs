using UnityEngine;
using System.Collections;

public class TileVisbility : MonoBehaviour
{
	[HideInInspector]
	public bool BlockSight;

	bool visible;
	bool discovered = false;

	SpriteRenderer[] renderers;
	Color[] colours;

	public bool Visible
	{
		get
		{
			return visible;
		}
	}

	public bool Discovered
	{
		get
		{
			return visible;
		}
	}

	void OnEnable()
	{
		if (MapBuildController.editing) return;
		renderers = GetComponentsInChildren<SpriteRenderer>();
		colours = new Color[renderers.Length];
		for (int i = 0; i < renderers.Length; i++)
			colours[i] = renderers[i].color;
		visible = true;
		Hide();
		if (GetComponent<IMapBlock>() != null)
		{
			BlockSight = GetComponent<IMapBlock>().BlockSight;
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
		if (MapBuildController.editing) return;
		if (teardown) return;
		SightRadius.Instance.RemoveTile(IntVector2.RoundFrom(transform.position));
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	public void Show()
	{
		if (visible == false)
		{
			visible = true;
			discovered = true;
			for (int i = 0; i < renderers.Length; i++)
			{
				renderers[i].color = colours[i];
				renderers[i].enabled = true;
			}
			foreach (MapObject o in ObjectMap.Instance.ObjectsAtLocation(IntVector2.RoundFrom(transform.position)))
			{
				o.VisibleToPlayer = true;
			}
		}
	}

	public void Hide()
	{
		if (visible == true)
		{
			visible = false;
			for(int i = 0; i < renderers.Length; i++)
			{
				if (discovered)
					renderers[i].color = Color.gray * colours[i];
				else
					renderers[i].enabled = false;
			}
			
			foreach (MapObject o in ObjectMap.Instance.ObjectsAtLocation(IntVector2.RoundFrom(transform.position)))
				o.VisibleToPlayer = false;
		}
	}
}
