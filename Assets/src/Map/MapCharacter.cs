using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MapObject))]
[RequireComponent(typeof(HitPoints))]
[RequireComponent(typeof(CircleCollider2D))]
public class MapCharacter : MonoBehaviour
{

	public bool raycastleft = false;

	// Use this for initialization
	void Start ()
	{
		gameObject.layer = LayerMask.NameToLayer("Character");
		GetComponent<CircleCollider2D>().isTrigger = true;
		// maybe get a health bar and shit
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Melee(MapCharacter target)
	{
		target.GetComponent<HitPoints>().Hurt(new DamageData().SetDamage(Random.Range(3, 6)));
	}
}
