using UnityEngine;
using System.Collections;

public class Crossbow : MonoBehaviour
{
	public int ReloadTime = 4;
	public int Damage = 7;

	MapCharacter user;
	MapObject me;

	int reloadTimer = 0;

	void Start()
	{
		user = GetComponent<MapCharacter>();
		me = GetComponent<MapObject>();
	}
	
	public bool CanTarget(MapCharacter target)
	{
		return LOSCheck.HasLOS(me, target.GetComponent<MapObject>());
	}

	public bool Loaded
	{
		get
		{
			return reloadTimer <= 0;
		}
	}

	public bool Attack(MapCharacter target)
	{
		if (!Loaded) return false;
		if (!CanTarget(target)) return false;

		if(Hit(target))
		{
			int range = IntVector2Utility.PFDistance(target.GetComponent<MapObject>().RealLocation, GetComponent<MapObject>().RealLocation);
			bool hit = Random.value < (user.hitSkill / (user.hitSkill + target.dodgeSkill/2 + range) );
			if (hit)
			{
				float damage = (0.75f + Random.value * 0.5f) * Damage * 20f / (20f + target.armor);  // hardcoding armour penetration!
				if (damage < 1f) damage = 1;
				target.GetComponent<HitPoints>().Hurt(new DamageData().SetDamage((int)damage));
			}
		}

		reloadTimer = ReloadTime;
		return true;
	}

	bool Hit(MapCharacter target)
	{
		return true;
	}

	/// <summary>
	/// Reload the crossbow
	/// </summary>
	/// <returns>True if the bow is ready to fire</returns>
	public bool Reload()
	{
		reloadTimer--;
		return Loaded;
	}
}
