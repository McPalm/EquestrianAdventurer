using UnityEngine;
using System.Collections;

public class GroundRandomItem : GroundItem
{
	public GroundItem[] common;
	public GroundItem[] uncommon;
	public GroundItem[] rare;

	public override Item CloneItem()
	{
		if(rare.Length > 0 && Random.value < 0.02f)
		{
			return rare[Random.Range(0, rare.Length)].CloneItem();
		}
		else if(uncommon.Length > 0 && Random.value < 0.2f)
		{
			return uncommon[Random.Range(0, uncommon.Length)].CloneItem();
		}
		else
		{
			return common[Random.Range(0, common.Length)].CloneItem();
		}
	}
}
