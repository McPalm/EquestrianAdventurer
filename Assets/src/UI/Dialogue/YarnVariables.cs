using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Yarn;

public class YarnVariables : Yarn.Unity.VariableStorageBehaviour
{
	Dictionary<string, Value> data = new Dictionary<string, Value>();

	public Purse purse;
	public Inventory playerInventory;

	public override void ResetToDefaults()
	{
		data = new Dictionary<string, Value>();
	}

	public override Value GetValue(string variableName)
	{
		if (variableName.Length > 6 && variableName.Substring(0, 6) == "$flag.")
		{
			bool f = StoryFlags.Instance.HasFlag(variableName.Substring(6));
			return new Value(f);
		}
		else if(variableName.Length > 11 && variableName.Substring(0, 11) == "$inventory.")
		{
			// if (variableName.Length > 19)
			if (variableName.Length > 19 && variableName.Substring(11, 8).ToLower() == "valuable")
			{
				if (playerInventory.Contains(variableName.Substring(20))) return new Value(true);
				if (playerInventory.Contains("pristine " + variableName.Substring(20))) return new Value(true);
				if (playerInventory.Contains("divine " + variableName.Substring(20))) return new Value(true);
				return new Value(false);
			}
			else if (variableName.Length > 17 && variableName.Substring(11, 6).ToLower() == "number")
			{
				string parseName = variableName.Substring(18);
				string exactName = "";
				for (int i = 0; i < parseName.Length; i++)
				{
					if (parseName[i] == '_') exactName += ' ';
					else exactName += parseName[i];
				}
				return new Value((float)playerInventory.Quantity(exactName));
			}
			else
			{
				string parseName = variableName.Substring(11);
				string exactName = "";
				for (int i = 0; i < parseName.Length; i++)
				{
					if (parseName[i] == '_') exactName += ' ';
					else exactName += parseName[i];
				}
				return new Value(playerInventory.Contains(exactName));
			}
		}
		Value r;
		if(data.TryGetValue(variableName, out r))
			return r;
		return Value.NULL;
	}

	public override void SetValue(string variableName, Value value)
	{
		if (variableName.Length > 6 && variableName.Substring(0, 6) == "$flag.")
		{
			if (value.AsBool)
				StoryFlags.Instance.AddFlag(variableName.Substring(6));
			else
				StoryFlags.Instance.RemoveFlag(variableName.Substring(6));
			return;
		}
		if(data.ContainsKey(variableName))
		{
			data.Remove(variableName);
		}
		data.Add(variableName, value);
	}

	public override float GetNumber(string variableName)
	{
		print("number?");
		if (variableName == "bits") return purse.bits;
		if (variableName == "day") return TimeAndDay.Instance.Day;
		if (variableName == "hour") return TimeAndDay.Instance.Hour;
		Value v;
		if(data.TryGetValue(variableName, out v))
		{
			return v.AsNumber;
		}
		return 0f;
	}

	public override void SetNumber(string variableName, float number)
	{
		if (variableName == "bits") Debug.LogError("Cannot assign bits through Set in dialogue");
		if (data.ContainsKey(variableName))
			data.Remove(variableName);
		data.Add(variableName, new Value(number));
	}
}
