﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Yarn;

public class DialogieUIYarn : Yarn.Unity.DialogueUIBehaviour
{
	[SerializeField]
	Text text;
	[SerializeField]
	Button[] optionButtons;
	[SerializeField]
	RectTransform buttonAnchor;
	[Space(10)]
	[SerializeField]
	float lettersPerSecond = 60f;
	[SerializeField]
	float bodyOptionPadding = 12f;

	float currentLineTime = 0f;
	OptionChooser currentChooser;
	RogueController player;
	string log;
	bool runLine;
	bool waiting = true;
	bool autoClose = false;

	public bool AutoClose
	{
		get
		{
			return autoClose;
		}
		set
		{
			autoClose = value;
		}
	}

	public override IEnumerator RunCommand(Command command)
	{
		switch (command.text.ToLower())
		{
			case "wait":
				optionButtons[0].GetComponentInChildren<Text>().text = "...";
				yield return Wait();
				yield break;
			case "clear":
					log = "";
				yield break;
			default: 
				yield return CommandParser.Instance.RunCommand(command.text);
				yield break;
		}
	}

	public override IEnumerator RunLine(Line line)
	{
		runLine = true;
		currentLineTime = Time.deltaTime;

		while(currentLineTime * lettersPerSecond < line.text.Length)
		{
			text.text = log + line.text.Substring(0, (int)(currentLineTime * lettersPerSecond));
			yield return null;
			currentLineTime += Time.deltaTime;
			if (Input.anyKeyDown) break;
			text.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 8f, text.preferredHeight);
		}

		text.text = log + line.text;

		/*
		do
		{
			yield return null;
		}
		while (!Input.anyKeyDown);
		*/

		log += line.text + "\n\n";
		yield return new WaitForSeconds(0.25f);
			 
	}

	public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
	{

		runLine = false;
		yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < optionsCollection.options.Count; i++)
        {
            optionButtons[i].GetComponentInChildren<Text>().text = "";
            // activate all options buttons before moving, or the text position wont sync properly
            optionButtons[i].gameObject.SetActive(true);
        }

        buttonAnchor.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, text.preferredHeight + bodyOptionPadding, 35);

        for (int i = 0; i < optionsCollection.options.Count; i++)
        {
            optionButtons[i].GetComponentInChildren<Text>().text = optionsCollection.options[i];
            yield return new WaitForSeconds(0.15f);
        }

        currentChooser = optionChooser;
		while (currentChooser != null)
			yield return null;
	}

	public void ChoseOption(int i)
	{
		if (currentChooser == null)
		{
			waiting = false;
		}
		else
		{
			currentChooser(i);
			foreach (Button b in optionButtons)
				b.gameObject.SetActive(false);

			log = "";

			currentChooser = null;
		}
	}

	public override IEnumerator DialogueComplete()
	{
		if(runLine &! autoClose)
		{
			optionButtons[0].GetComponentInChildren<Text>().text = "end";
			yield return Wait();
		}

		gameObject.SetActive(false);
		player.enabled = true;
		yield return new WaitForSeconds(0f);
	}

	public override IEnumerator DialogueStarted()
	{
		gameObject.SetActive(true);
		text.text = "";
		player = FindObjectOfType<RogueController>();
		player.enabled = false;
		log = "";
		autoClose = false;
		yield break;
	}

	public IEnumerator Wait()
	{
        optionButtons[0].gameObject.SetActive(true); // we must activate gameobject before we move parents, or it wont work properly.
        buttonAnchor.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, text.preferredHeight + bodyOptionPadding, 35);
		waiting = true;
		do
		{
			yield return null;
		}
		while (waiting);
		optionButtons[0].gameObject.SetActive(false);
	}
}
