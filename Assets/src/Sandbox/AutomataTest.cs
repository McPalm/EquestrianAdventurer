using UnityEngine;
using System.Collections;

public class AutomataTest : MonoBehaviour
{

	public bool generate = false;
	bool nextUpdate = false;

	

	// Update is called once per frame
	void Update ()
	{
		if(generate)
		{
			//CellularAutomata generator = new CellularAutomata();

			RoomChain5by5 generator = new RoomChain5by5();

			generator.Generate();

			GetComponent<MapSection>().LoadFromBlueprint(generator.GetResult());

			generate = false;
			nextUpdate = true;
		}
		else if(nextUpdate)
		{
			nextUpdate = false;
			GetComponent<CreatureSpawner>().Spawn();
		}
	}
}
