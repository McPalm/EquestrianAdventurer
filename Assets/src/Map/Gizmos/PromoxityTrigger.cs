using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// triggers an event when the players is close enough
/// </summary>

public class PromoxityTrigger : MonoBehaviour
{
	[SerializeField]
	MapObject MustBeVisible;

	public UnityEvent EventFire = new UnityEvent();

	// Use this for initialization
	void Start ()
	{
		// FindObjectOfType<RogueController>();
		StartCoroutine(CheckLoop());
	}
	
	IEnumerator CheckLoop()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.25f);
			if(ShouldFire())
			{
				EventFire.Invoke();
				break;
			}
		}
	}

	bool ShouldFire()
	{
		if (MustBeVisible && MustBeVisible.VisibleToPlayer)
		{
			return true;
		}
		return false;
	}
}
