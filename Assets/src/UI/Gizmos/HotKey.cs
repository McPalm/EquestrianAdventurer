using UnityEngine;
using UnityEngine.Events;
using System.Collections;




public class HotKey : MonoBehaviour
{

	public KeyCode key;
	public UnityEvent onPress;


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			onPress.Invoke();
		}
	}
}
