using UnityEngine;
using System.Collections;

public class SnapToMouse : MonoBehaviour
{
	void Update()
	{
		Vector3 des = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(Mathf.Round(des.x), Mathf.Round(des.y));
	}
}
