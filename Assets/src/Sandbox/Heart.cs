using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		Stats a = new Stats();

		a.armor = 0;
		print(a.DamageReduction(0));

		a.armor = 0;
		print(a.DamageReduction(10));

		a.armor = 10;
		print(a.DamageReduction(0));

		a.armor = 10;
		print(a.DamageReduction(10));

		a.armor = 20;
		print(a.DamageReduction(0));

		a.armor = 20;
		print(a.DamageReduction(10));
	}
}
