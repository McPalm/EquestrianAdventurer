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

	/*
	 * public void Save()
	{
		if (!dirty) return;
		try
		{
			XmlSerializer xml = new XmlSerializer(typeof(CharacterSheet));
			Debug.Log("Writing to: " + Application.persistentDataPath +  "/" + name + ".xml");
			TextWriter writer = new StreamWriter(Application.persistentDataPath + "/" + name + ".xml");
			xml.Serialize(writer, this);
			writer.Close();
			dirty = false;
		}
		catch(Exception e)
		{
			Debug.LogException(e);
		}
	}

	static public CharacterSheet Load(string name)
	{
		try
		{
			XmlSerializer xml = new XmlSerializer(typeof(CharacterSheet));
			XmlTextReader reader = new XmlTextReader(Application.persistentDataPath + "/" + name + ".xml");
			CharacterSheet sheet = xml.Deserialize(reader) as CharacterSheet;
			reader.Close();
			sheet.dirty = false;
			return sheet;
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
		return new CharacterSheet(name);
	}
	*/
}
