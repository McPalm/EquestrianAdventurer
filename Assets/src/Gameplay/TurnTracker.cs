using UnityEngine;
using System.Collections.Generic;

public class TurnTracker : MonoBehaviour
{

	static TurnTracker _instance;

	List<SimpleBehaviour> characters;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		characters = new List<SimpleBehaviour>(FindObjectsOfType<SimpleBehaviour>());
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
}
