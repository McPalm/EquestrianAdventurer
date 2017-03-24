using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class MapSectionData
{
	static public int DIMENSIONS = 41;

	public int[][] tiles = new int[DIMENSIONS][];
	public MapSectionData() { }

	public string name;

	public MapSectionData(string name)
	{
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
		try
		{
			XmlSerializer xml = new XmlSerializer(typeof(MapSectionData));
			Debug.Log("Writing to: " + Application.persistentDataPath + "/mapsections/" + name + ".xml");
			TextWriter writer = new StreamWriter(Application.persistentDataPath + "/mapsections/" + name + ".xml");
			xml.Serialize(writer, this);
			writer.Close();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	static public MapSectionData Load(string name)
	{
		try
		{
			XmlSerializer xml = new XmlSerializer(typeof(MapSectionData));
			XmlTextReader reader = new XmlTextReader(Application.persistentDataPath + "/mapsections/" + name + ".xml");
			MapSectionData section = xml.Deserialize(reader) as MapSectionData;
			reader.Close();
			return section;
		}
		catch (Exception)
		{
			// Debug.LogException(e);
		}
		return new MapSectionData(name);
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
