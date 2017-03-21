using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour
{
	[SerializeField]
	Transform drag;
	Color baseColor;

	void Awake()
	{
		baseColor = GetComponent<SpriteRenderer>().color;
	}

	public void SetScale(int current, int max)
	{
		if (max == 0) max = 1; // divide by zero
		if (current < 0) current = 0;
		transform.localScale = new Vector3(current / (float)max, 1f, 1f);
		StopAllCoroutines();
		if (drag)
			StartCoroutine(AnimateDrag(current / (float)max));
		StartCoroutine(DamageFlash());
	}

	IEnumerator AnimateDrag(float targetValue)
	{
		float startValue = drag.localScale.x;
		yield return new WaitForSeconds(0.75f);
		
		for(int i = 0; i < 20; i++)
		{
			drag.localScale = new Vector3(Mathf.Lerp(startValue, targetValue, i / 20f), 1f);
			yield return new WaitForSeconds(0.0333333f);
		}
		drag.localScale = new Vector3(targetValue, 1f);
	}

	IEnumerator DamageFlash()
	{
		GetComponent<SpriteRenderer>().color = Color.white;
		yield return new WaitForSeconds(0.03f);
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, baseColor, 0.33f);

		yield return new WaitForSeconds(0.03f);
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, baseColor, 0.77f);

		yield return new WaitForSeconds(0.03f);
		GetComponent<SpriteRenderer>().color = baseColor;
	}
}
