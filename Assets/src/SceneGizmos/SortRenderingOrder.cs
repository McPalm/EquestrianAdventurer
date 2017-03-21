/*
 * @ author: Palm
 * This script will give items a rendering priority based on its current position along the y axis.
 * 
 */

using UnityEngine;
using System.Collections;

public class SortRenderingOrder : MonoBehaviour {

	public int mod = 0;
	public bool startOnly = false;

	void Start(){
		if(startOnly){
			SortMe();
			Destroy(this);
		}
	}

	// Update is called once per frame
	void Update () {
		SortMe();
	}

	void OnDrawGizmosSelected(){
		if(!Application.isPlaying){
			SortMe();
		}
	}

	private void SortMe()
	{
		SpriteRenderer rend = GetComponent<SpriteRenderer>();
		rend.sortingOrder = 1 - Mathf.RoundToInt(transform.position.y*10) + mod;
	}
}


