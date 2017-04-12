using UnityEngine;
using System.Collections;

public class DropOnDeath : MonoBehaviour
{
	public GameObject[] Common = new GameObject[0];
	public GameObject[] Uncommon = new GameObject[0];
	public GameObject[] Rare = new GameObject[0];


	// Use this for initialization
	void Start ()
	{
		GetComponent<MapCharacter>().EventDeath.AddListener(OnDeath);
	}
	
	// Update is called once per frame
	void OnDeath(MapCharacter me)
	{
		

		if(Rare.Length > 0 && Random.value < 0.02f)
		{
			Instantiate(Rare[Random.Range(0, Rare.Length)], (Vector3)GetComponent<MapObject>().RealLocation, Quaternion.identity);
		}
		else if (Uncommon.Length > 0 && Random.value < 0.15f)
		{
			Instantiate(Uncommon[Random.Range(0, Uncommon.Length)], (Vector3)GetComponent<MapObject>().RealLocation, Quaternion.identity);
		}
		else if (Common.Length > 0)
		{
			Instantiate(Common[Random.Range(0, Common.Length)], (Vector3)GetComponent<MapObject>().RealLocation, Quaternion.identity);
		}
	}
}
