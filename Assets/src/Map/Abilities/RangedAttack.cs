using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RangedAttack : MonoBehaviour
{
	public int Range = 10;

	protected MapCharacter user;
	protected MapObject me;
	protected LOSCheck LoS;

	public MapCharacterEvent EventOnHit = new MapCharacterEvent();
	public MapCharacterEvent EventOnMiss = new MapCharacterEvent();

	// Use this for initialization
	protected void Start ()
	{
		user = GetComponent<MapCharacter>();
		me = GetComponent<MapObject>();
		LoS = GetComponent<LOSCheck>();
	}

	virtual public bool CanTarget(MapCharacter target)
	{
		return LoS.HasLOE(target.GetComponent<MapObject>(), Range);
	}

	virtual public bool Useable
	{
		get
		{
			return true;
		}
	}

	virtual protected bool Hit(MapCharacter target)
	{
		int range = IntVector2Utility.PFDistance(target.GetComponent<MapObject>().RealLocation, GetComponent<MapObject>().RealLocation);
		return Random.value < (user.hitSkill / (user.hitSkill + ((range == 1) ? target.dodgeSkill : target.dodgeSkill / 2) + range));
	}

	public bool Attack(MapCharacter target)
	{
		if (!Useable) return false;
		if (!CanTarget(target)) return false;
		if (Hit(target)) EventOnHit.Invoke(target);
		else EventOnMiss.Invoke(target);
		return true;
	}

	[System.Serializable]
	public class MapCharacterEvent : UnityEvent<MapCharacter> { }
}
