using UnityEngine;
using System.Collections;

public class DialogueStarter : MonoBehaviour
{
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
