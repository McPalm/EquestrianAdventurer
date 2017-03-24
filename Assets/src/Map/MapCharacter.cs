using UnityEngine;
using UnityEngine.Events;
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

	public GameObject skull;

	public UnityEvent EventDeath = new UnityEvent();

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
			float damage = (0.75f + Random.value * 0.5f) * baseDamage * baseDamage / (baseDamage + target.armor);
			target.GetComponent<HitPoints>().Hurt(new DamageData().SetDamage((int)damage));
			HurtPool.Instance.DoHurt(target.GetComponent<MapObject>().RealLocation, (int)damage);
		}
		else
			CombatTextPool.Instance.PrintAt((Vector3)target.GetComponent<MapObject>().RealLocation + new Vector3(0f, 0.4f), "Dodge", Color.cyan);
		transform.position = (transform.position + target.transform.position) * 0.5f;
		GetComponent<Mobile>().ForceMove((Vector2)GetComponent<MapObject>().RealLocation);
	}

	void OnHurt(int current, int max)
	{
		if (current <= 0)
		{
			Destroy(gameObject);
			Instantiate(skull, transform.position, Quaternion.identity);
			EventDeath.Invoke();
		}
	}
}
