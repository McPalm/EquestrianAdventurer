using UnityEngine;
using System.Collections;

public class SimpleCombatMessage : MonoBehaviour
{
	public string message;

	public void PrintMessage()
	{
		PrintMessage(message);
	}

	public void PrintMessage(string m)
	{
		CombatTextPool.Instance.PrintAt(transform.position + new Vector3(0f, 0.65f), m, Color.white, 1f + m.Length * 0.07f);
	}
}
