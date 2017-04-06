using UnityEngine;
using System.Collections.Generic;

public class TurnTracker : MonoBehaviour
{

	static TurnTracker _instance;

	List<TurnEntry> characters = new List<TurnEntry>();
	List<TurnEntry> remove = new List<TurnEntry>();

	bool iterating = false;

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
		iterating = true;
		foreach (TurnEntry sb in characters)
		{
			sb.DoTurn();
		}
		iterating = false;
		foreach (TurnEntry r in remove)
			characters.Remove(r);
		remove.Clear();
	}

	public void Add(TurnEntry e)
	{
		characters.Add(e);
	}

	public void Remove(TurnEntry e)
	{
		if (iterating) remove.Add(e);
		else characters.Remove(e);
	}
	
	public interface TurnEntry
	{
		void DoTurn();
	}
}
