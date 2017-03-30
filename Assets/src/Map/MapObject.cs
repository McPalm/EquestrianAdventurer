using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour
{
	MapCharacter mapCharacter;
	IntVector2 realLocation;

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
			ObjectMap.Instance.Move(this, realLocation, value);
			realLocation = value;
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
	}

	void OnApplicationQuit()
	{
		on = false;	
	}

	bool on = true;
	void OnDisable()
	{
		if(on) ObjectMap.Instance.Remove(this);
	}
}
