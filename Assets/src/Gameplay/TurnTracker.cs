using UnityEngine;
using System.Collections.Generic;

public class TurnTracker : MonoBehaviour
{

	static TurnTracker _instance;

	List<TurnEntry> characters = new List<TurnEntry>();

	void Awake()
	{
		_instance = this;
	}

	public static TurnTracker Instance
	{
		get
		{
			return _instance;
		}
	}

	public void NextTurn()
	{
		foreach (SimpleBehaviour sb in characters)
			sb.DoTurn();
	}

	public void Add(TurnEntry e)
	{
		characters.Add(e);
	}

	public void Remove(TurnEntry e)
	{
		characters.Remove(e);
	}
	
	public interface TurnEntry
	{
		void DoTurn();
	}
}
