using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MapObject))]
[RequireComponent(typeof(HitPoints))]
[RequireComponent(typeof(CircleCollider2D))]
public class MapCharacter : MonoBehaviour
{
	public float hitSkill;
	public float baseDamage;
	public float dodgeSkill;
	public float armor;

	public bool raycastleft = false;

	// Use this for initialization
	void Start ()
	{
		gameObject.layer = LayerMask.NameToLayer("Character");
		GetComponent<CircleCollider2D>().isTrigger = true;
		// maybe get a health bar and shit
	}

	public void Melee(MapCharacter target)
	{
		bool hit = Mathf.Min(Random.value, Random.value) < (hitSkill / (target.dodgeSkill + hitSkill));
		if (hit)
		{
			float damage = baseDamage * baseDamage / (baseDamage + target.armor);
			target.GetComponent<HitPoints>().Hurt(new DamageData().SetDamage((int)damage));
		}
	}
}
