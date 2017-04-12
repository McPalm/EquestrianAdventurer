using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SpawnOnMove : MyBehaviour
{

	public GameObject Prefab;

	public UnityEvent EventMove = new UnityEvent();

	// Use this for initialization
	void Start ()
	{
		EventDisable.AddListener(MyDisable);
	}
	
	void OnMove(CharacterActionController cac, CharacterActionController.Actions a)
	{
		if((a & CharacterActionController.Actions.movement) != 0)
		{
			Instantiate(Prefab, (Vector3)GetComponent<MapObject>().RealLocation, Quaternion.identity);
			EventMove.Invoke();
		}
	}

	void OnEnable()
	{
		GetComponent<CharacterActionController>().EventAfterAction.AddListener(OnMove);
	}

	void MyDisable()
	{
		GetComponent<CharacterActionController>().EventAfterAction.RemoveListener(OnMove);
	}


}
