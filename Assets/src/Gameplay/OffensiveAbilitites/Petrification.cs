using UnityEngine;
using System.Collections;

public class Petrification : MonoBehaviour
{
	[Range(0f, 1f)]
	public float chance = 1f;

	public void Petrify(GameObject o)
	{
		if(Random.value < chance)
			if(false == o.GetComponent<Petrified>())
				o.AddComponent<Petrified>();
	}
}
