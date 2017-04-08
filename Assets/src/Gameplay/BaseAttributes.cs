using UnityEngine;
using System.Collections;

[System.Serializable]
public struct BaseAttributes
{
	public int Strenght;
	public int Dexterity;
	public int Agility;
	public int Endurance;
	public int Tenacity;
	public int Luck;

	public BaseAttributes(int Strenght, int Dexterity, int Agility, int Endurance, int Tenacity, int Luck)
	{
		this.Strenght = Strenght;
		this.Dexterity = Dexterity;
		this.Agility = Agility;
		this.Endurance = Endurance;
		this.Tenacity = Tenacity;
		this.Luck = Luck;
	}

	public Stats Stats
	{
		get
		{
			return new Stats(Strenght + Dexterity + Agility + Endurance + Tenacity*3 + Luck, Dexterity, Agility, 0, Strenght, 0, 0, Luck, Luck + Agility);
		}
	}

	static public BaseAttributes operator + (BaseAttributes a, BaseAttributes b)
	{
		return new BaseAttributes(a.Strenght + b.Strenght, a.Dexterity + b.Dexterity, a.Agility + b.Agility, a.Endurance + b.Endurance, a.Tenacity + b.Tenacity, a.Luck + b.Luck);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="substring">put at 0 if you wish to start with a new row</param>
	/// <returns></returns>
	public string NeatStringSkipEmpty(int substring = 1)
	{
		string s = "";
		if (Strenght != 0) s += "\nStrenght: " + Strenght;
		if (Dexterity != 0) s += "\nDexterity: " + Dexterity;
		if (Agility != 0) s += "\nAgility: " + Agility;
		if (Endurance != 0) s += "\nEndurance: " + Endurance;
		if (Tenacity != 0) s += "\nTenacity: " + Tenacity;
		if (Luck != 0) s += "\nLuck: " + Luck;
		return s.Substring(substring);
	}

	public string NeatString()
	{
		return
			"Strenght: " + Strenght +
			"\nDexterity: " + Dexterity +
			"\nAgility: " + Agility +
			"\nEndurance: " + Endurance +
			"\nTenacity: " + Tenacity + 
			"\nLuck: " + Luck
			;
	}
}
