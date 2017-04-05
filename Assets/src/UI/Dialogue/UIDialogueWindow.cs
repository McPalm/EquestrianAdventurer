using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIDialogueWindow : MonoBehaviour
{
	static UIDialogueWindow _instance;

	[SerializeField]
	Transform window;
	[SerializeField]
	Transform anchor;
	[SerializeField]
	Text textArea;
	[SerializeField]
	Text characterName;
	[SerializeField]
	Image portrait;
	[SerializeField]
	Button askButton;


	List<Button> buttons = new List<Button>();
	DialogueData d;
	Transform buttonAnchor;

	HashSet<string> local = new HashSet<string>();
	HashSet<string> global = new HashSet<string>();

	public void Open(string fileName, string name, Sprite sprite)
	{
		local.Clear();
		if(DialogueData.TryLoad(fileName, out d))
		{
			window.gameObject.SetActive(true);
			window.position = anchor.position;
			characterName.text = name;
			portrait.sprite = sprite;
			TalkAbout("Hello");
		}
		else
		{
			Debug.LogError("No dialogue for " + fileName);
		}
	}
	
	public void Close()
	{
		window.gameObject.SetActive(false);
	}

	public void Close(string name)
	{
		if (characterName.text == name)
			Close();
	}

	public void TalkAbout(string keyword)
	{
		DialogueSection text;
		if(d.TryGetText(keyword, out text))
		{
			textArea.text = text.body;
			local.UnionWith(text.local);
			global.UnionWith(text.global);
		}
		else
		{
			textArea.text = "I dont have anything to say about that.";
		}
		BuildQuestions();
	}

	public static UIDialogueWindow Instance
	{
		get
		{
			return _instance;
		}
	}

	public void Awake()
	{
		_instance = this;
		window.gameObject.SetActive(false);
		buttons.Add(askButton);
		buttonAnchor = askButton.transform.parent;
	}

	public void BuildQuestions()
	{
		int count = 0;
		foreach(string key in d.AllKeys)
		{
			if (local.Contains(key) || global.Contains(key))
			{
				string capturedKey = key; // why does this matter? I thought strings were all cached and immutable
				if (count == buttons.Count)
				{
					buttons.Add(Instantiate(askButton));
					buttons[count].transform.SetParent(buttonAnchor);
					buttons[count].transform.position = buttonAnchor.transform.position + new Vector3(0f, -36f) * count;
				}
				buttons[count].gameObject.SetActive(true);
				buttons[count].onClick.RemoveAllListeners();
				buttons[count].GetComponentInChildren<Text>().text = key;
				buttons[count].onClick.AddListener(
					delegate ()
					{
						TalkAbout(capturedKey);
					}
					);
				count++;
			}
		}
		for (int i = count; i < buttons.Count; i++)
			buttons[i].gameObject.SetActive(false);
	}
}
