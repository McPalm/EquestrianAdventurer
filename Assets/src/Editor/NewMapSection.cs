using UnityEngine;
using UnityEditor;
using System.Collections;

public class NewMapSection : EditorWindow
{
	[MenuItem("EQA/New Map Section")]
	// Use this for initialization
	static void Init ()
	{
		NewMapSection window = (NewMapSection)GetWindow(typeof(NewMapSection));
		window.Show();
	}

	string filename;
	Color tint = Color.gray;
	string tileset;


	void OnGUI()
	{
		// GUILayout.Label("Enter Name", EditorStyles.boldLabel);
		filename = EditorGUILayout.TextField("Name", filename);
		tileset = EditorGUILayout.TextField("Tileset", tileset);
		tint = EditorGUILayout.ColorField("Screen Tint", tint);


		if (GUILayout.Button("Create") && filename.Length > 2)
		{
			// verify stuffs
			bool pass = true;
			if(MapSectionData.TryGet(filename) != null)
			{
				Debug.LogError("Name already in use!");
				pass = false;
			}
			if (TileSet.GetTileSet(tileset) == null)
			{
				Debug.LogError("Missing tileset: " + tileset);
				pass = false;
			}

			if (pass)
			{
				MapSectionData data = new MapSectionData(filename);
				data.tint = tint;
				data.palette = tileset;
				data.Save();
				Close();
			}
		}
		if (GUILayout.Button("Cancel"))
		{
			Close();
		}
	}
}
