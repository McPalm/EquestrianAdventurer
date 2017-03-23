using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageBox : MonoBehaviour
{
	public Text text;
	public Image background;

	public Sprite[] backgroundSprites;

	public Color color1;
	public Color color2;

	public void Display(Vector3 target, int value)
	{
		gameObject.SetActive(true);
		text.text = value.ToString();
		transform.position = Camera.main.WorldToScreenPoint(target + new Vector3(Random.value * 0.6f - 0.3f, Random.value * 0.6f - 0.1f));

		background.sprite = backgroundSprites[Random.Range(0, backgroundSprites.Length)];
		background.color = Color.Lerp(color1, color2, Random.value);
		StartCoroutine(KillAfterSeconds(0.6f));
	}

	IEnumerator KillAfterSeconds(float time)
	{
		yield return new WaitForSeconds(time);
		gameObject.SetActive(false);
	}
}
