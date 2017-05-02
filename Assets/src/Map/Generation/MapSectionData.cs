using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class MapSectionData
{
	static string resourcePath = "XML/Maps/";

	static public int DIMENSIONS = 41;

	public int[][] tiles; // = new int[DIMENSIONS][];
	public MapSectionData() { }
	public IntVector2 moduleAnchor = new IntVector2(21, 21);

	public string name;

	public string palette = "";
	public SerializedColor tint = new SerializedColor(0.5f, 0.5f, 0.5f, 1f);

	public MapSectionData(string name)
	{
		tiles = new int[DIMENSIONS][];
		for (int x = 0; x < DIMENSIONS; x++)
		{
			tiles[x] = new int[DIMENSIONS];
			for (int y = 0; y < DIMENSIONS; y++)
				tiles[x][y] = -1;
		}
		this.name = name;
	}

	public void Save()
	{
		XmlTool.EditorSaveObjectAsXML(this, resourcePath + name);
	}

	static public MapSectionData Load(string name)
	{
		// MapSectionData data = new MapSectionData();
		return XmlTool.LoadFromXML<MapSectionData>(resourcePath + name);
	}

	static public MapSectionData TryGet(string name)
	{
		// MapSectionData data = new MapSectionData();
		try
		{
			return XmlTool.LoadFromXML<MapSectionData>(resourcePath + name, false);
		}
		catch
		{
			return null;
		}

	}
}
