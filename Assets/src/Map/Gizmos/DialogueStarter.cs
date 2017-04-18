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
		/*
		if(GetComponent<SpriteRenderer>())
			UIDialogueWindow.Instance.Open(fileName, name, GetComponent<SpriteRenderer>().sprite, gameObject);
		else
			UIDialogueWindow.Instance.Open(fileName, name, null, gameObject);
			*/
		Yarn.Unity.DialogueRunner d = FindObjectOfType<Yarn.Unity.DialogueRunner>();
		//d.sourceText = new TextAsset[]{ file };
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
