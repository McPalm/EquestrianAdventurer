using UnityEngine;
using System.Collections;

abstract public class Aura : MonoBehaviour
{

	public Sprite Icon;
	public Color IconColor = Color.white;

	abstract public Stats Stats{ get; }
	abstract public BaseAttributes Attributes { get; }
	abstract public string IconText { get; }
	abstract public string Tooltip { get; }

	void Start()
	{
		if(GetComponent<RogueController>())
		{
			AuraIconManager.Instance.Add(this);
		}
		GetComponent<MapCharacter>().Refresh();
	}
}
