using UnityEngine;
using System.Collections;

public class ScanForEnemies : MonoBehaviour
{
	MapCharacter me;
	LOSCheck los;
	RangedAI ai;

	int interval = 2;

	void Start()
	{
		ai = GetComponent<RangedAI>();
		ai.PreTurnEvent.AddListener(Scan);
		me = GetComponent<MapCharacter>();
		los = GetComponent<LOSCheck>();
	}

	void Scan()
	{
		interval--;
		if (interval > 0) return;
		//print("scan");
		MapObject target = ai.target;

		IntVector2 l = GetComponent<MapObject>().RealLocation;
		foreach (MapObject mo in ObjectMap.Instance.GetRange(l.x - ai.relaxedRadius, l.y - ai.relaxedRadius, l.x + ai.relaxedRadius, l.y + ai.relaxedRadius))
		{
			//print(mo);
			MapCharacter c = mo.GetComponent<MapCharacter>();
			if (c && me.HostileTowards(c) && los.HasLOS(mo, ai.Relaxed, true))
			{
				//print("potential target");
				if (target == null)
					target = mo;
				else if(IntVector2Utility.DeltaSum(l, mo.RealLocation) < IntVector2Utility.DeltaSum(l, target.RealLocation))
					target = mo;
			}
		}

		ai.target = target;
		interval = 3;
	}
}
