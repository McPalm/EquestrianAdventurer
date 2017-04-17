using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Yarn;

public class YarnVariables : Yarn.Unity.VariableStorageBehaviour
{
	Dictionary<string, Value> data = new Dictionary<string, Value>();

	public Purse purse;


	public override void ResetToDefaults()
	{
		data = new Dictionary<string, Value>();
	}



	public override Value GetValue(string variableName)
	{
		if(variableName.Length > 5 && variableName.Substring(0, 5) == "flag.")
		{
			bool f = StoryFlags.Instance.HasFlag(variableName.Substring(5));
			return new Value(f);
		}
		Value r;
		if(data.TryGetValue(variableName, out r))
			return r;
		return Value.NULL;
	}

	public override void SetValue(string variableName, Value value)
	{
		if (variableName.Length > 5 && variableName.Substring(0, 5) == "flag.")
		{
			if (value.AsBool)
				StoryFlags.Instance.AddFlag(variableName.Substring(5));
			else
				StoryFlags.Instance.RemoveFlag(variableName.Substring(5));
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
		if (variableName == "bits") return purse.bits;
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
