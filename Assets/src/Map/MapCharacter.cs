using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mobile))]
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
		GetComponent<MapObject>().MapCharacter = this;
		GetComponent<HitPoints>().EventChangeHealth.AddListener(OnHurt);
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

	void OnHurt(int current, int max)
	{
		if (current <= 0)
			Destroy(gameObject);
	}
}
