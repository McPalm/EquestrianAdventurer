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
		 + "\n\nAttack: " + character.baseDamage
		 + "\nHit: " + character.hitSkill
		 + "\nArmor: " + character.armor + " (" + Mathf.Round(100f * (1f - 10f / (10f + character.armor))) + "%)"
		 + "\nDodge: " + character.dodgeSkill;
	}
}
