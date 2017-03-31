using UnityEngine;
using System.Collections.Generic;

public class BlockMap : MonoBehaviour
{
	private static BlockMap _instance;

	Dictionary<IntVector2, IMapBlock> blockage = new Dictionary<IntVector2, IMapBlock>();

	public static BlockMap Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<BlockMap>();
			return _instance;
		}
	}


	public void Add(IMapBlock b, GameObject o)
	{
		blockage.Add(IntVector2.RoundFrom(o.transform.position), b);
	}

	public void Remove(GameObject o)
	{
		blockage.Remove(IntVector2.RoundFrom(o.transform.position));
	}

	public bool BlockMove(Vector2 v2)
	{
		return BlockMove(IntVector2.RoundFrom(v2));
	}

	public bool BlockSight(Vector2 v2)
	{
		return BlockSight(IntVector2.RoundFrom(v2));
	}

	public bool BlockMove(IntVector2 v2)
	{
		IMapBlock imb = null;
		if (blockage.TryGetValue(v2, out imb))
			return imb.BlockMove;
		return false;
	}

	public bool BlockSight(IntVector2 v2)
	{
		IMapBlock imb = null;
		if (blockage.TryGetValue(v2, out imb))
			return imb.BlockSight;
		return false;
	}
}
