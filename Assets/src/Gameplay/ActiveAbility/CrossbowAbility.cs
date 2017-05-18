using UnityEngine;
using System.Collections;

public class CrossbowAbility : RangedAbility
{
	bool loaded = true;
	[SerializeField]
	MapCharacter character;

	/*
	[SerializeField]
	Sprite reloadIcon;
	*/

	public override bool CanUse
	{
		get
		{
			return true;
		}
	}

	public bool Loaded
	{
		get
		{
			return loaded;
		}

		protected set
		{
			// TODO, change the sproite
			loaded = value;
		}
	}

	public override bool CanUseAt(IntVector2 targetLocation)
	{
		if(Loaded)
			return base.CanUseAt(targetLocation);
		return true;
	}

	public override bool TryUseAt(IntVector2 targetLocation)
	{
		if (Loaded)
		{
			GetComponent<Hurt>().damage = 5 + User.Attributes.Dexterity / 2;
			if(base.TryUseAt(targetLocation))
			{
				Loaded = false;
				return true;
			}
			return false;
		}
		else
		{
			// reloading
			CharacterActionController cac = User.GetComponent<CharacterActionController>();
			cac.StackAction(CharacterActionController.Actions.ability);
			cac.StackAction(CharacterActionController.Actions.ability);
			cac.StackAction(CharacterActionController.Actions.ability);
			CombatTextPool.Instance.PrintAt(transform.position, "reloading...", Color.white);
			Loaded = true;
			return true;
		}
	} 
}
