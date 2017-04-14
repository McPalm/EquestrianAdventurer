using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddNumberDisplay : MonoBehaviour {

	public float TimeOut = 2f;
	public string suffix;

	public GameObject frame;
	public Text text;

	public Color positiveColor = Color.white;
	public Color negativeColor = Color.white;

	int currentSum;

	public void Add(int i)
	{
		currentSum += i;
		frame.SetActive(true);
		if(currentSum > 0) text.text = "+" + currentSum.ToString() +  suffix;
		else text.text = currentSum.ToString() + suffix;
		text.color = (currentSum < 0) ? negativeColor : positiveColor;
		StopAllCoroutines();
		StartCoroutine(ResetIn(TimeOut));
	}

	public void Substract(int i)
	{
		Add(-i);
	}

	IEnumerator ResetIn(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Reset();
	}

	void Reset()
	{
		frame.SetActive(false);
		currentSum = 0;
	}
}
