using UnityEngine;
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

	float currentLineTime = 0f;
	OptionChooser currentChooser;

	public override IEnumerator RunCommand(Command command)
	{
		print(command.text);
		yield break;
	}

	public override IEnumerator RunLine(Line line)
	{
		currentLineTime = Time.deltaTime;

		while(currentLineTime * lettersPerSecond < line.text.Length)
		{
			text.text = line.text.Substring(0, (int)(currentLineTime * lettersPerSecond));
			yield return null;
			currentLineTime += Time.deltaTime;
			if (Input.anyKeyDown) break;
		}

		text.text = line.text;

		do
		{
			yield return null;
		}
		while (!Input.anyKeyDown);

	}

	public override IEnumerator RunOptions(Options optionsCollection, OptionChooser optionChooser)
	{
		buttonAnchor.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, text.preferredHeight + 12f, 35);

		for (int i = 0; i < optionsCollection.options.Count; i++)
		{
			optionButtons[i].GetComponentInChildren<Text>().text = optionsCollection.options[i];
			optionButtons[i].gameObject.SetActive(true);
		}
		currentChooser = optionChooser;
		while (currentChooser != null)
			yield return new WaitForSeconds(0f);
	}

	public void ChoseOption(int i)
	{
		currentChooser(i);
		foreach (Button b in optionButtons)
			b.gameObject.SetActive(false);

		

		currentChooser = null;
	}

	public override IEnumerator DialogueComplete()
	{
		gameObject.SetActive(false);
		yield return new WaitForSeconds(0f);
	}

	public override IEnumerator DialogueStarted()
	{
		gameObject.SetActive(true);
		text.text = "";
		yield break;
	}
}
