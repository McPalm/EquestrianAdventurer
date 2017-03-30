using UnityEditor;
using UnityEngine;
using System.Collections;

public class MyEditor : Editor
{


	protected string TextField(string label, string value)
	{
		string temp = EditorGUILayout.TextField(label, value);
		if (temp != value)
		{
			Undo.RecordObject(target, label);
			//EditorUtility.SetDirty(target);
		}
		return temp;
	}

	protected bool Toggle(string label, bool value)
	{
		bool temp = EditorGUILayout.Toggle(label, value);
		if(temp != value)
		{
			Undo.RecordObject(target, label);
			//EditorUtility.SetDirty(target);
		}
		return temp;
	}

	protected Color ColorField(Color c, string undo)
	{
		Color temp = EditorGUILayout.ColorField(c);
		if(temp != c)
		{
			Undo.RecordObject(target, undo);
		}
		return temp;
	}

	protected int IntSlider(string label, int value, int min, int max)
	{
		int temp = EditorGUILayout.IntSlider(label, value, min, max);
		if(temp != value)
		{
			Undo.RecordObject(target, label);
		}
		return temp;
	}
}
