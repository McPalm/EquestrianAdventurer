using UnityEngine;
using System.Collections;

abstract public class Aura : MonoBehaviour
{

	public Sprite Icon;
	public Color IconColor = Color.white;

	abstract public Stats Stats{ get; }
}
