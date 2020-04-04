using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

public static class XmlTool
{
	public static string GetXMLString<T>(T target)
	{
		/*
		// remove the default namespaces
		XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
		ns.Add(string.Empty, string.Empty);*/
		// serialize to string

		XmlSerializer xs = new XmlSerializer(typeof(T));
		StringWriter sw = new StringWriter();
		xs.Serialize(sw, target);
		return sw.GetStringBuilder().ToString();
	}

	/// <summary>
	/// This is only to be used with the editor.
	/// DOES NOT WORK IN PUBLISHED BUILDS
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="target"></param>
	/// <param name="path"></param>
	public static void EditorSaveObjectAsXML<T>(T target, string path)
	{
		try
		{
			XmlSerializer xml = new XmlSerializer(typeof(T));
			path = Application.dataPath + "/resources/" + path + ".xml";
			Debug.Log("Writing to: " + path);
			TextWriter writer = new StreamWriter(path);
			xml.Serialize(writer, target);
			writer.Close();
#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
#endif
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	public static T LoadFromXML<T>(string pathInResources, bool logException = true)
	{
		try
		{
			Debug.Log("Loading from: " + pathInResources);
			TextAsset temp = Resources.Load(pathInResources) as TextAsset;
            Debug.Log(temp.text);
			return LoadFromXMLString<T>(temp.text);
		}
		catch (Exception e)
		{
			if(logException) Debug.LogException(e);
			throw new Exception("unable to generate file.");
		}
	}

	public static T LoadFromXMLString<T>(string s)
	{
        Stream stream = null;

        try
		{
			XmlSerializer xml = new XmlSerializer(typeof(T));
			stream = GenerateStreamFromString(s);
			T file = (T)xml.Deserialize(stream);
			stream.Close();
			return file;
		}
		catch (Exception e)
		{
			Debug.LogException(e);
            stream?.Close();
            throw new Exception("unable to generate file.");
		}
	}

	public static Stream GenerateStreamFromString(string s)
	{
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(s);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}

}