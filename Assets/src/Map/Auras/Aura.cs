using UnityEngine;
using System.Collections;

abstract public class Aura : MonoBehaviour
{

	public Sprite Icon;
	public Color IconColor = Color.white;
	public string tooltip;

	abstract public Stats Stats{ get; }
	abstract public string IconText { get; }
}
