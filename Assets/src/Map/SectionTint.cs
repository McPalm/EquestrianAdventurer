using UnityEngine;
using System.Collections;

public class SectionTint : MonoBehaviour
{
	public PostProcessingTest postprocessor;
	public float lerpDuration = 5f;

	public void OnNewSection(MapSection s)
	{
		if(s)
		{
			StartCoroutine(StartTransition(s.overlayTint));
		}
	}

	IEnumerator StartTransition(Color c)
	{
		Color startColor = postprocessor.color;

		for(float f = 0; f < lerpDuration; f += Mathf.Max(Time.deltaTime, 0.1f))
		{
			postprocessor.color = Color.Lerp(startColor, c, f / lerpDuration);
			yield return new WaitForSeconds(0f);
		}
		postprocessor.color = c;
	}
}
