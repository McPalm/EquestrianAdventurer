using UnityEngine;
using System.Collections;

public abstract class AActiveAbility : MonoBehaviour
{
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

	public string AbilityName;
	[TextArea]
	public string Description;
}
