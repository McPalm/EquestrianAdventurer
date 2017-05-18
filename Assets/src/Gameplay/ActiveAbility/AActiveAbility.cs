using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public abstract class AActiveAbility : MonoBehaviour
{
	protected MapCharacter User
	{
		get
		{
			return GetComponentInParent<MapCharacter>();
		}
	}
	protected Mobile Me
	{
		get
		{
			return GetComponentInParent<Mobile>();
		}
	}
	protected StaminaPoints Stamina
	{
		get
		{
			return GetComponentInParent<StaminaPoints>();
		}
	}
	protected LOSCheck LOS
	{
		get
		{
			return GetComponentInParent<LOSCheck>();
		}
	}

	/// <summary>
	/// if the target location is viable to use the skill at
	/// </summary>
	/// <param name="targetLocation"></param>
	/// <returns></returns>
	abstract public bool CanUseAt(IntVector2 targetLocation);
	/// <summary>
	/// actually try to use the ability, true if it went through
	/// </summary>
	/// <param name="targetLocation"></param>
	/// <returns>True if we use the ability</returns>
	abstract public bool TryUseAt(IntVector2 targetLocation);
	/// <summary>
	/// if we meet all the conditions to use the ability
	/// </summary>
	abstract public bool CanUse { get; }
	/// <summary>
	/// if we meet all the conditions to use the ability
	/// </summary>
	abstract public Sprite Icon { get; }
	/// <summary>
	/// Called if we need to change the icon during runetime for some reason.
	/// </summary>
	internal SpriteEvent SetIcon = new SpriteEvent();

	public string AbilityName;
	[TextArea]
	public string Description;

	public class SpriteEvent : UnityEvent<Sprite> { }
}
