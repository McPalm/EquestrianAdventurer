using UnityEngine;
using System.Collections;

public class StoryDialoguePicker : MonoBehaviour
{
	[SerializeField]
	string[] requiredFlags;
	[SerializeField]
	TextAsset dialogueFile;
	[SerializeField]
	bool onceOnly = false;
	[SerializeField]
	string completedFlag;

	public TextAsset DialogueFile
	{
		get
		{
			return dialogueFile;
		}
	}

	public bool ShouldPlay()
	{
		if (StoryFlags.Instance.HasFlag(completedFlag))
			return false;
		foreach (string flag in requiredFlags)
			if (false == StoryFlags.Instance.HasFlag(flag)) return false;
		return true;
	}

	public void Played()
	{
		if (onceOnly)
			StoryFlags.Instance.AddFlag(completedFlag);
	}
}
