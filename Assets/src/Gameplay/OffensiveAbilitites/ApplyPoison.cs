using UnityEngine;
using System.Collections;

public class ApplyPoison : MonoBehaviour {

	public int duration;
	public int damage;
	[Range(0f, 1f)]
	public float chance;
	public Sprite icon;

	public void PoisonTarget(GameObject o)
	{
		if (Random.value < chance)
			Poison.StackPoison(o, duration, damage, icon);
	}
}
