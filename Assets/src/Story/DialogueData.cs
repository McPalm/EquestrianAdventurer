using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
	static public string PATH = "XML/dialogues/";

	public string fileName;

	Dictionary<string, DialogueSection> library; // = new Dictionary<string, string>();
	public Snippet[] allDialogues; // for saving and loading. Is usually null

	/// <summary>
	/// Used by the XMLserializer, use LoadOrCreate instead
	/// </summary>
	public DialogueData()
	{ }

	DialogueData(string fileName)
	{
		this.fileName = fileName;
		library = new Dictionary<string, DialogueSection>();
	}

	public bool TryGetText(string keyword, out DialogueSection text)
	{
		return library.TryGetValue(keyword, out text);
	}

	public bool ContainsKey(string keyword)
	{
		return library.ContainsKey(keyword);
	}

	public void Write(string keyword, DialogueSection text)
	{
		if (library.ContainsKey(keyword))
			library.Remove(keyword);
		library.Add(keyword, text);
	}

	public void Write(string keyword, string text)
	{
		DialogueSection d;
		if (library.TryGetValue(keyword, out d))
		{
			d.body = text;
		}
		else
		{
			d = new DialogueSection(keyword);
			d.body = text;
			d.AddLocal("new local");
			library.Add(keyword, d);
		}
	}

	public bool RemoveKeyword(string keyword)
	{
		return library.Remove(keyword);
	}

	[System.Xml.Serialization.XmlIgnore]
	public IEnumerable<string> AllKeys
	{
		get
		{
			return library.Keys;
		}
	}

	public void Save()
	{
		serialize();
		XmlTool.EditorSaveObjectAsXML(this, PATH + fileName);
		allDialogues = null;
	}

	public void SaveAs(string fileName)
	{
		this.fileName = fileName;
		Save();
	}

	static public bool TryLoad(string fileName, out DialogueData dialogue)
	{
		try
		{
			dialogue = XmlTool.LoadFromXML<DialogueData>(PATH + fileName, false);
			dialogue.deserialize();
		}
		catch
		{
			dialogue = null;
			return false;
		}
		return true;
	}

	static public DialogueData LoadOrCreate(string fileName)
	{
		DialogueData d;
		if (TryLoad(fileName, out d))
		{
			d.deserialize();
			return d;
		}
		return new DialogueData(fileName);
	}

	private void deserialize()
	{
		if(library == null)
		{
			library = new Dictionary<string, DialogueSection>();
			foreach(Snippet c in allDialogues)
			{
				library.Add(c.keyword, c.section);
			}
			allDialogues = null;
		}
	}

	private void serialize()
	{
		List<Snippet> list = new List<Snippet>();
		foreach(KeyValuePair<string, DialogueSection> pair in library)
		{
			Snippet c = new Snippet();
			c.keyword = pair.Key;
			c.section = pair.Value;
			list.Add(c);
		}
		allDialogues = list.ToArray();
	}

	public bool Initialized
	{
		get
		{
			return library != null;
		}
	}

	[System.Serializable]
	public struct Snippet
	{
		public string keyword;
		public DialogueSection section;
	}

	public class StringStringDictionary : Dictionary<string, DialogueSection>
	{

	}
}
