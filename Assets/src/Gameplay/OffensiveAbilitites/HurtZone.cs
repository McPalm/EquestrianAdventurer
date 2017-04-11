using UnityEngine;
using System.Collections;


public class HurtZone : MyBehaviour, TurnTracker.TurnEntry
{
	public float damage;
	public DamageTypes damageType;
	public int duration;

	public bool scale = true;
	public float startSize;
	public string stackTag = "";

	public void DoTurn()
	{
		MapCharacter mc = ObjectMap.Instance.CharacterAt(GetComponent<MapObject>().RealLocation);
		if(mc)
		{
			float currentDamage = (scale) ? damage * duration / startSize : damage;
			DamageData d = new DamageData(gameObject)
				.AddType(damageType)
				.SetDamage(currentDamage * Random.Range(0.75f, 1.25f));
			mc.GetComponent<HitPoints>().Hurt(d);
		}
		if (duration > 0)
		{
			duration--;
			if (duration == 0) Destroy(gameObject);
			else if(scale)
			{
				transform.localScale = Vector3.one * duration / startSize;
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		if(stackTag != "")
		{
			foreach(MapObject o in ObjectMap.Instance.ObjectsAtLocation(IntVector2.RoundFrom(transform.position)))
			{
				HurtZone other = o.GetComponent<HurtZone>();
				if (other && other != this)
				{
					if(other.stackTag == stackTag)
					{
						if (other.duration < duration)
							other.duration = duration;
						Destroy(gameObject);
						return;
					}
				}
			}
		}


		TurnTracker.Instance.Add(this);
		EventDestroy.AddListener(MyDisable);
		if(scale)
		{
			startSize = duration;
		}
	}

	void MyDisable()
	{
		TurnTracker.Instance.Remove(this);
	}
}
