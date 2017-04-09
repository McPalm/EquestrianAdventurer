using UnityEngine;
using System.Collections;

public class SlowMovement : MyBehaviour
{
	public int duration = -1;

	// Use this for initialization
	void Awake()
	{
		EventDisable.AddListener(
			() =>
			{
				GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnAfterAction);
			});
	}

	void OnEnable()
	{
		GetComponent<CharacterActionController>().EventAfterAction.AddListener(OnAfterAction);
	}

	void OnAfterAction(CharacterActionController cac, CharacterActionController.Actions action)
	{
		if(duration > 0)
		{
			duration--;
			if (duration == 0) Destroy(this);
		}

		if((action & CharacterActionController.Actions.movement) != 0)
		{
			cac.root++;
		}

		/*
		if (action != CharacterActionController.Actions.none && action != CharacterActionController.Actions.idle)
			cac.StackAction(CharacterActionController.Actions.none);
			*/
	}
}
