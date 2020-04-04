using UnityEngine;
using System.Collections;

public class DialogueStarter : MonoBehaviour
{
	public TextAsset jsonFile;

	[SerializeField]
	StoryDialoguePicker[] StoryDialogues;


	public void StartConversation()
	{
		foreach (StoryDialoguePicker option in StoryDialogues)
		{
			if (option.ShouldPlay())
			{
				StartConversation(option.DialogueFile);
				option.Played();
				return;
			}
		}
		if(jsonFile)
			StartConversation(jsonFile);
	}

	public void StartConversation(TextAsset file)
	{
		CommandParser.Instance.talkTo = gameObject;

		Yarn.Unity.DialogueRunner d = FindObjectOfType<Yarn.Unity.DialogueRunner>();

		MapObject o = GetComponent<MapObject>();
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		UIDialogueWindow.Instance.Setup(
			(o) ? o.displayName : name,
			(sr) ? sr.sprite : null
			);
		

		d.Clear();
		d.AddScript(file);
		d.StartDialogue();
	}

	public void Close()
	{
        Debug.Log("Close invoked");
        UIDialogueWindow.Instance.Close();
	}
}
