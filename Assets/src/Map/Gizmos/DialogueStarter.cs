using UnityEngine;
using System.Collections;

public class DialogueStarter : MonoBehaviour
{
	public TextAsset jsonFile;


	public void StartConversation()
	{
		StartConversation(jsonFile);
	}


	public void StartConversation(TextAsset file)
	{
		CommandParser.Instance.talkTo = gameObject;

		Yarn.Unity.DialogueRunner d = FindObjectOfType<Yarn.Unity.DialogueRunner>();
		
		d.Clear();
		d.AddScript(file);
		d.StartDialogue();
	}

	public void Close()
	{
		// UIDialogueWindow.Instance.Close(name);
		Debug.LogWarning("Not Implemented!");
	}
}
