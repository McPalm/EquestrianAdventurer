﻿using UnityEngine;
using System.Collections;

public class DialogueStarter : MonoBehaviour
{
	public string defaultFile;


	public void StartConversation()
	{
		StartConversation(defaultFile);
	}


	public void StartConversation(string fileName)
	{
		if(GetComponent<SpriteRenderer>())
			UIDialogueWindow.Instance.Open(fileName, name, GetComponent<SpriteRenderer>().sprite);
		else
			UIDialogueWindow.Instance.Open(fileName, name, null);
	}

	public void Close()
	{
		UIDialogueWindow.Instance.Close(name);
	}
}
