using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RangedAbility : AActiveAbility
{
	[SerializeField]
	Sprite icon;

	[Space(10)]
	public int staminaCost = 0;
	public int maxRange = 6;
	public bool requiresLineOfSight = true;
	[Space(10)]
	public bool targetCreature = true;
	public bool targetItem = false;
	public bool targetGround = false;
	public bool useAccuracy = true;

	public GameObjectEvent OnHit = new GameObjectEvent();
	public GameObjectEvent OnMiss = new GameObjectEvent();
	public GameObjectEvent OnUse = new GameObjectEvent();

	public IntVector2Event OnHitLocation = new IntVector2Event();
	public IntVector2Event OnMissLocation = new IntVector2Event();
	public IntVector2Event OnUseLocation = new IntVector2Event();

	public override bool CanUse
	{
		get
		{
			if (staminaCost > 0) return Stamina.CurrentStamina >= staminaCost;
			return true;
		}
	}

	public override Sprite Icon
	{
		get
		{
			return icon;
		}
	}

	public override bool CanUseAt(IntVector2 targetLocation)
	{
		bool valid;
		if(requiresLineOfSight)
			valid = CanUse && LOS.HasLOE(targetLocation, maxRange);
		else 
			valid = (targetLocation - Me.RealLocation).MagnitudePF <= maxRange;
		if (!valid) return false;

		
		if (targetCreature && ObjectMap.Instance.CharacterAt(targetLocation)) return true;
		if (targetGround && !BlockMap.Instance.BlockMove(targetLocation)) return true;
		if (targetItem && ObjectMap.Instance.ObjectsAtLocation(targetLocation).Length > 0) return true;

		return false;
	}

	public override bool TryUseAt(IntVector2 targetLocation)
	{
		if (!CanUseAt(targetLocation)) return false;
		if (!Stamina.TryPay(staminaCost)) return false;

		MapCharacter target = ObjectMap.Instance.CharacterAt(targetLocation);

		if (targetCreature && useAccuracy)
		{
			if(target)
			{
				if(Random.value < User.Stats.HitChance(target.Stats))
				{
					OnHit.Invoke(target.gameObject);
					OnHitLocation.Invoke(targetLocation);
				}
				else
				{
					OnMiss.Invoke(target.gameObject);
					OnMissLocation.Invoke(targetLocation);
				}

				OnUse.Invoke(target.gameObject);
			}
		}
		else if(targetCreature && target)
		{
			OnUse.Invoke(target.gameObject);
		}
		if(targetItem)
		{
			foreach(MapObject o in ObjectMap.Instance.ObjectsAtLocation(targetLocation))
			{
				if (o.GetComponent<GroundItem>())
					OnUse.Invoke(o.gameObject);
			}
		}
		OnUseLocation.Invoke(targetLocation);
		return true;
	}

	[System.Serializable]
	public class GameObjectEvent : UnityEvent<GameObject> { }
	[System.Serializable]
	public class IntVector2Event : UnityEvent<IntVector2> { }
}
