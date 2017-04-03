using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// using System;


public class GUIBar : MonoBehaviour
{
	[SerializeField]
	RectTransform Bar;
	[SerializeField]
	RectTransform Drag;
	[SerializeField]
	Image Flash;
	[SerializeField]
	Text text;
	[SerializeField]
	RectTransform.Edge Anchor;
	Color baseColor;

	int old;
	float maxSize;

	void Awake()
	{
		maxSize = Bar.sizeDelta.x;
		baseColor = Flash.color;
	}

	public void SetScale(int current, int max)
	{
		if (max == 0) max = 1; // divide by zero
		if (current < 0) current = 0;
		// transform.localScale = new Vector3(current / (float)max, 1f, 1f);
		// Bar.SetSizeWithCurrentAnchors(Anchor, maxSize * current / max);
		Bar.SetInsetAndSizeFromParentEdge(Anchor, 0f, maxSize * current / max);
		StopAllCoroutines();
		if (Drag)
			StartCoroutine(AnimateDrag(current / (float)max));
		if(old > current) StartCoroutine(DamageFlash());
		else Flash.color = baseColor;
		old = current;

		text.text = current + "/" + max;
	}

	IEnumerator AnimateDrag(float targetValue)
	{
		float startValue = Drag.sizeDelta.x;
		float endValue = targetValue * maxSize;
		yield return new WaitForSeconds(0.75f);

		for (float t = 0f; t < 1f; t += Time.deltaTime)
		{
			// Drag.SetSizeWithCurrentAnchors(Anchor, Mathf.Lerp(startValue, endValue, t));
			Drag.SetInsetAndSizeFromParentEdge(Anchor, 0f, Mathf.Lerp(startValue, endValue, t));
			// drag.localScale = new Vector3(Mathf.Lerp(startValue, targetValue, i / 20f), 1f);
			yield return new WaitForSeconds(0f);
		}
		Drag.SetInsetAndSizeFromParentEdge(Anchor, 0f, endValue);
	}

	IEnumerator DamageFlash()
	{
		Flash.color = Color.white;
		yield return new WaitForSeconds(0.03f);
		Flash.color = Color.Lerp(Color.white, baseColor, 0.33f);

		yield return new WaitForSeconds(0.03f);
		Flash.color = Color.Lerp(Color.white, baseColor, 0.77f);

		yield return new WaitForSeconds(0.03f);
		Flash.color = baseColor;
	}
}
