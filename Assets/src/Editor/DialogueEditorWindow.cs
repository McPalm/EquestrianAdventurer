using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogueEditorWindow : EditorWindow
{
	// Add menu named "My Window" to the Window menu
	// [MenuItem("EQA/Dialogue Editor")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		DialogueEditorWindow window = (DialogueEditorWindow)EditorWindow.GetWindow(typeof(DialogueEditorWindow));
		window.Show();
	}

	static string file;
	static DialogueData data;

	void OnGUI()
	{
		GUILayout.Label("Hello World", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		file = EditorGUILayout.TextField("File Name", file);
		if(GUILayout.Button("Save"))
		{
			data.Save();
			file = data.fileName;
		}
		if(GUILayout.Button("Load") && file.Length > 2)
		{
			data = DialogueData.LoadOrCreate(file);
		}
		EditorGUILayout.EndHorizontal();
		if (data == null || !data.Initialized) 
		{
			GUILayout.Label("No file loaded.");
		}
		else
		{
			string label = "";
			string write = "";
			foreach (string key in data.AllKeys)
			{
				string b;
				if (DisplayData(key, out b))
				{
					label = key;
					write = b;
				}
			}
			if(label != "")
				data.Write(label, write);
		}
		/*
		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle("Toggle", myBool);
		myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup();
		*/
	}

	bool DisplayData(string key, out string s)
	{
		string z;
		data.TryGetText(key, out z);
		GUILayout.BeginHorizontal();
		GUILayout.Label(key); // GUILayout.MaxWidth(75f)
		s = GUILayout.TextArea(z);
		GUILayout.EndHorizontal();
		return s != z;
	}
	
}
