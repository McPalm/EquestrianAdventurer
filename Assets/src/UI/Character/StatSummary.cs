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
		Level level = character.GetComponent<Level>();
		text.text = "Max HP: " + character.GetComponent<HitPoints>().MaxHealth
		+ "\nLevel: " + level.level
		+ "\nExperience: " + level.experience
		+ "\nNext Level: " + level.NextLevel
		 + "\n\nAttack: " + character.Stats.damage
		 + "\nPen: " + character.Stats.armorpen
		 + "\nHit: " + character.Stats.hit
		 + "\nArmor: " + character.Stats.armor + " (" + Mathf.Round(100f * character.Stats.DamageReduction(0)) + "%)"
		 + "\nDodge: " + character.Stats.dodge;
	}
}
