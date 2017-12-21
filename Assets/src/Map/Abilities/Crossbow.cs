using UnityEngine;
using System.Collections;

public class Crossbow : RangedAttack
{
	public int ReloadTime = 4;
	public int Damage = 7;
	public int ArmorPen = 7;

	new protected void Start()
	{
		EventOnHit.AddListener(OnHit);
		EventOnMiss.AddListener(OnMiss);
		base.Start();
	}

	int reloadTimer = 0;

	public override bool Useable
	{
		get
		{
			return Loaded;
		}
	}

	public bool Loaded
	{
		get
		{
			return reloadTimer <= 0;
		}
	}

	void OnHit(MapCharacter target)
	{
		DamageData data = new DamageData(gameObject)
			.SetDamage(Damage * Random.Range(0.75f, 1.25f), Damage * 0.75f)
			.SetArmorPen(ArmorPen);
		target.GetComponent<HitPoints>().Hurt(data);
		HurtPool.Instance.DoHurt(target.GetComponent<MapObject>().RealLocation, data.TotalDamage);
		reloadTimer = ReloadTime;
		NoiseUtility.CauseNoise(4, GetComponent<MapObject>().RealLocation);
	}

	void OnMiss(MapCharacter target)
	{
		NoiseUtility.CauseNoise(4, GetComponent<MapObject>().RealLocation);
		CombatTextPool.Instance.PrintAt(transform.position + new Vector3(0f, 0.4f), "Miss", Color.cyan);
		reloadTimer = ReloadTime;
	}

	/// <summary>
	/// Reload the crossbow
	/// </summary>
	/// <returns>True if the bow is ready to fire</returns>
	public bool Reload()
	{
		if (Loaded) return true;
		reloadTimer--;
		float percentage = (float)(ReloadTime - reloadTimer) / (float)ReloadTime * 100f;
		CombatTextPool.Instance.PrintAt((Vector3)me.RealLocation + new Vector3(0f, 0.4f), "Reloading " + Mathf.Round(percentage) + "%", Color.cyan);
		return Loaded;
	}
}
