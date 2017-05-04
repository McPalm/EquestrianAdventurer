using UnityEngine;
using UnityEditor;
using System.Collections;

public class NewDialogue : EditorWindow
{


	// [MenuItem("EQA/New Dialogue")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		NewDialogue window = (NewDialogue)GetWindow(typeof(NewDialogue));
		window.Show();
	}

	string filename;

	void OnGUI()
	{
		GUILayout.Label("Enter Name", EditorStyles.boldLabel);
		filename = EditorGUILayout.TextField("File Name", filename);
		if (GUILayout.Button("Create") && filename.Length > 2)
		{
			DialogueData d = DialogueData.LoadOrCreate(filename);
			d.Write("Hello", "Hello!");
			d.Save();
			Close();
		}
		if (GUILayout.Button("Cancel"))
		{
			Close();
		}
	}
}