using UnityEngine;
using System.Collections;

// put on a canvass to signal that held dropable items should be put in this canvass

public class DropCanvas : MonoBehaviour
{
	public static Transform Canvas;


	void Awake()
	{
		Canvas = transform;
	}
}
