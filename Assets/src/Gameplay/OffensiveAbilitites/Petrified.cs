using UnityEngine;
using System.Collections;

public class Petrified : MyBehaviour
{
	int timer = 5;
	bool blockNextMessage = false;

	void Start()
	{
		EventDisable.AddListener(Remove);
	}

	void OnEnable()
	{
		GetComponent<CharacterActionController>().EventAfterAction.AddListener(OnEndTurn);
	}

	void Remove()
	{
		GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnEndTurn);
	}

	void OnEndTurn(CharacterActionController c, CharacterActionController.Actions a)
	{
		timer--;
		if (timer <= 0)
		{
			CombatTextPool.Instance.PrintAt(transform.position, "You are now a statue.", new Color(0.9f, 0.45f, 0.5f));
			GetComponent<HitPoints>().CurrentHealth = 0;
			Destroy(this);
		}
		else
		{
			if (timer > 3)
				CombatTextPool.Instance.PrintAt(transform.position, "Cant feel your hooves", new Color(1f, 0.25f, 0.3f));
			else
			{
				if (!blockNextMessage)
				{
					if (timer == 2) CombatTextPool.Instance.PrintAt(transform.position, "Your tail is now stone", new Color(1f, 0.25f, 0.3f));
					else if(timer == 1) CombatTextPool.Instance.PrintAt(transform.position, "Your hooves are now stone", new Color(1f, 0.35f, 0.4f));
				}
				blockNextMessage = false;
				if (Random.value < 0.5f)
				{
					c.StackAction(CharacterActionController.Actions.none);
					blockNextMessage = true;
				}
			}
		}
	}
}
