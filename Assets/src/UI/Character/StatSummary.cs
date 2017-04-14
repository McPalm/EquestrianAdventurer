using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatSummary : MonoBehaviour
{
	public Text text;
	public MapCharacter character;



	// Use this for initialization
	void Start ()
	{
		CalcStats(character);
		character.EventUpdateStats.AddListener(CalcStats);
	}
	
	
	void CalcStats (MapCharacter character)
	{
		text.text = character.Attributes.NeatString()
			
		 + "\n\nMax HP: " + character.GetComponent<HitPoints>().MaxHealth
		 + "\nAttack: " + character.Stats.damage
		 + "\nPen: " + character.Stats.armorpen
		 + "\nHit: " + character.Stats.hit
		 + "\nArmor: " + character.Stats.armor + " (" + Mathf.Round(100f - 100f * character.Stats.DamageReduction(0)) + "%)"
		 + "\nDodge: " + character.Stats.dodge
		 + "\nCrit Chance: " + character.Stats.critChance
		 + "\nCrit Avoid: " + character.Stats.critAvoid;
		 
	}
}
