using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
	static public string PATH = "dialogues/";

	public string fileName;

	Dictionary<string, string> library; // = new Dictionary<string, string>();
	public MyContainer[] containers; // for saving and loading. Is usually null

	/// <summary>
	/// Used by the XMLserializer, use LoadOrCreate instead
	/// </summary>
	public DialogueData()
	{ }

	DialogueData(string fileName)
	{
		this.fileName = fileName;
	}

	public bool TryGetText(string keyword, out string text)
	{
		return library.TryGetValue(keyword, out text);
	}

	public bool ContainsKey(string keyword)
	{
		return library.ContainsKey(keyword);
	}

	public void AddText(string keyword, string text)
	{
		library.Add(keyword, text);
	}

	public bool RemoveKeyword(string keyword)
	{
		return library.Remove(keyword);
	}

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
		containers = null;
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
			dialogue = XmlTool.LoadFromXML<DialogueData>(PATH + fileName);
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
			return d;
		return new DialogueData();
	}

	private void deserialize()
	{
		if(library == null)
		{
			library = new Dictionary<string, string>();
			foreach(MyContainer c in containers)
			{
				library.Add(c.key, c.text);
			}
			containers = null;
		}
	}

	private void serialize()
	{
		List<MyContainer> list = new List<MyContainer>();
		foreach(KeyValuePair<string, string> pair in library)
		{
			MyContainer c = new MyContainer();
			c.key = pair.Key;
			c.text = pair.Value;
			list.Add(c);
		}
		containers = list.ToArray();
	}

	[System.Serializable]
	public class MyContainer
	{
		public string key, text;
	}
}
