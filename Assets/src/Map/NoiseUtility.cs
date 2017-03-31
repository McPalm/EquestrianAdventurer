using UnityEngine;
using System.Collections.Generic;

static public class NoiseUtility
{
	

	public static void CauseNoise(int volume, IntVector2 origin)
	{
		List<IntVector2> list = new List<IntVector2>();
		List<int> noiseLevel = new List<int>();
		HashSet<IntVector2> added = new HashSet<IntVector2>();

		BlockMap b = BlockMap.Instance;
		ObjectMap o = ObjectMap.Instance;

		list.Add(origin);
		noiseLevel.Add(volume);
		added.Add(origin);
		
		for(int i = 0; i < list.Count; i++)
		{

			if (o.CharacterAt(list[i]))
			{
				o.CharacterAt(list[i]).EventHearNoise.Invoke(origin, noiseLevel[i]);
			}

			if (noiseLevel[i] <= 1) continue;

			if (!added.Contains(list[i] + IntVector2.up) && !b.BlockMove(list[i] + IntVector2.up))
			{
				list.Add(list[i] + IntVector2.up);
				noiseLevel.Add(noiseLevel[i] - 1);
				added.Add(list[i] + IntVector2.up);
			}
			if (!added.Contains(list[i] + IntVector2.down) && !b.BlockMove(list[i] + IntVector2.down))
			{
				list.Add(list[i] + IntVector2.down);
				noiseLevel.Add(noiseLevel[i] - 1);
				added.Add(list[i] + IntVector2.down);
			}
			if (!added.Contains(list[i] + IntVector2.left) && !b.BlockMove(list[i] + IntVector2.left))
			{
				list.Add(list[i] + IntVector2.left);
				noiseLevel.Add(noiseLevel[i] - 1);
				added.Add(list[i] + IntVector2.left);
			}
			if (!added.Contains(list[i] + IntVector2.right) && !b.BlockMove(list[i] + IntVector2.right))
			{
				list.Add(list[i] + IntVector2.right);
				noiseLevel.Add(noiseLevel[i] - 1);
				added.Add(list[i] + IntVector2.right);
			}
		}
	}
}
