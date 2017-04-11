using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour
{
	MapCharacter mapCharacter;
	IntVector2 realLocation;
	bool visibleToPlayer;
	public string displayName;

	/// <summary>
	/// The objects real location as far as the game is concerned.
	/// This does not sync with apparent position, if you wish to move the graphic, use Put(Vector2)
	/// </summary>
	public IntVector2 RealLocation
	{
		get
		{
			return realLocation;
		}
		set
		{
			if (!enabled) return;
			ObjectMap.Instance.Move(this, realLocation, value);
			realLocation = value;
			VisibleToPlayer = SightRadius.Instance.LocationVisible(realLocation);

		}
	}

	public bool VisibleToPlayer
	{
		set
		{
			visibleToPlayer = value;
			foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
				sr.enabled = value;
		}
		get
		{
			return visibleToPlayer;
		}
	}

	public MapCharacter MapCharacter
	{
		get
		{
			return mapCharacter;
		}

		set
		{
			mapCharacter = value;
		}
	}

	public bool HasConcealment
	{
		get
		{
			return Concealment.ConcealmentAt(realLocation);
		}
	}

	/// <summary>
	/// Move the object graphic to a specified location
	/// </summary>
	/// <param name="v2"></param>
	public void Put(Vector2 v2)
	{
		transform.position = v2;
		RealLocation = IntVector2.RoundFrom(v2);
		if (this is Mobile) (this as Mobile).Stop();
	}

	void OnEnable()
	{
		realLocation = IntVector2.RoundFrom(transform.position);
		ObjectMap.Instance.Add(this);
		if (!GetComponent<RogueController>())
			VisibleToPlayer = false; // hide outside LOS
	}

	void Start()
	{
		VisibleToPlayer = SightRadius.Instance.LocationVisible(realLocation);
	}

	void OnApplicationQuit()
	{
		on = false;	
	}

	bool on = true;
	void OnDisable()
	{
		if (on)
		{
			ObjectMap.Instance.Remove(this);
		}
	}
}
