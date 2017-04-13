using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Return : MonoBehaviour
{

	public IntVector2 returnPoint;
	public int countDown = -1;
	public UnityEvent EventBeforeReturn = new UnityEvent();
	public UnityEvent EventAfterReturn = new UnityEvent();

	public float disabledDuration = 2f;


	// Use this for initialization
	void Start ()
	{
		GetComponent<CharacterActionController>().EventAfterAction.AddListener(Tick);
	}

	public void ReturnImmediate()
	{
		EventBeforeReturn.Invoke();
		GetComponent<Mobile>().ForceMove(returnPoint, 0f);
		EventAfterReturn.Invoke();
		StartCoroutine(DisableControl(disabledDuration));
	}

	public void ReturnTimer(int rounds)
	{
		if (rounds > 0)
			countDown = rounds;
		else if (rounds == 0)
			ReturnImmediate();
	}

	public void Tick(CharacterActionController cac, CharacterActionController.Actions a)
	{
		if(countDown > 0)
		{
			countDown--;
			if(countDown == 0)
			{
				countDown = -1;
				ReturnImmediate();
			}
		}
	}

	IEnumerator DisableControl(float seconds)
	{
		RogueController c = GetComponent<RogueController>();
		if(c)
		{
			c.enabled = false;
			yield return new WaitForSeconds(seconds);
			c.enabled = true;
		}
	}
}
