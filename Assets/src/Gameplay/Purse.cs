using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class Purse : MonoBehaviour
{
	public int bits;

	public PurseEvent EventChangeAmmount;
	public PurseEvent EventPaySum;
	public PurseEvent EventGainSum;
	public PurseStringEvent EventChangeAmmountText;


	void Start()
	{
		EventChangeAmmount.Invoke(bits);
		EventChangeAmmountText.Invoke(bits.ToString() + " bits");
	}


	public bool Pay(int i)
	{
		if (bits < i) return false;
		bits -= i;
		EventChangeAmmount.Invoke(bits);
		EventChangeAmmountText.Invoke(bits.ToString() + " bits");
		EventPaySum.Invoke(i);
		return true;
	}

	public void AddBits(int i)
	{
		bits += i;
		EventChangeAmmount.Invoke(bits);
		EventChangeAmmountText.Invoke(bits.ToString() + " bits");
		EventGainSum.Invoke(i);
	}

	public void Tax(float tax)
	{
		if (tax > 1f) tax = 1f;
		if (tax < 0f) return;
		int lose = (int)(bits * tax);
		Pay(lose);
	}

	[System.Serializable]
	public class PurseEvent : UnityEvent<int> { }
	[System.Serializable]
	public class PurseStringEvent : UnityEvent<string> { }
}
