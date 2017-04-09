using UnityEngine;
using System.Collections;

/// <summary>
/// Flies in the opposite direction when struck
/// </summary>
public class Puntable : MyBehaviour
{

	[Range(0f, 1f)]
	public float chance;

	

	void Awake()
	{
		EventDisable.AddListener(
			() =>
			{
				GetComponent<HitPoints>().EventBeforeHurt.RemoveListener(Struck);
			});
	}

	// Use this for initialization
	void OnEnable()
	{
		GetComponent<HitPoints>().EventBeforeHurt.AddListener(Struck);
	}
	

	public void Struck(DamageData d)
	{
		if (Random.value > chance) return;
		if(d.source)
		{
			MapObject o = d.source.GetComponent<MapObject>();
			Mobile me = GetComponent<Mobile>();
			if(o)
			{
				if (o.RealLocation.DeltaSum(me.RealLocation) == 1)
				{
					if (me.MoveDirection(me.RealLocation - o.RealLocation))
					{
						GetComponent<CharacterActionController>().root++;
						CombatTextPool.Instance.PrintAt(transform.position, "Punt!", Color.yellow);
					}
				}
			}
		}
	}
}
