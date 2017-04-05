[System.Serializable]
public class DialogueSection
{
	public string body;
	public string[] local = new string[0];
	public string[] global = new string[0];
	public string[] flags = new string[0];

	public DialogueSection() { }

	public void AddLocal(string s)
	{
		string[] n = new string[local.Length + 1];
		for(int i = 0; i < local.Length; i++)
		{
			n[i] = local[i];
		}
		n[n.Length - 1] = s;
		local = n;
	}

	public void AddGlobal(string s)
	{
		string[] n = new string[global.Length + 1];
		for (int i = 0; i < global.Length; i++)
		{
			n[i] = global[i];
		}
		n[n.Length - 1] = s;
		global = n;
	}
}
