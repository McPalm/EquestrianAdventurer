using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float travelingTime = 1f;
	public float stick = 1f;

	public GameObject prefab;
	public StrechBetweenTwoObjects trailPrefab;

	Transform projectile;
	StrechBetweenTwoObjects trail;
	Transform tailEnd;

	public void Start()
	{
		projectile = Instantiate(prefab).transform;
		projectile.gameObject.SetActive(false);
		if(trailPrefab)
		{
			trail = Instantiate(trailPrefab);
			trail.gameObject.SetActive(false);
			trail.target = projectile;
			tailEnd = new GameObject("tail end").transform;
			trail.origin = tailEnd;
		}
	}

	public void FireAt(Component c)
	{
		projectile.transform.position = transform.position;
		StartCoroutine(Animate(c.transform, projectile, travelingTime, stick));
	}

	IEnumerator Animate(Transform goal, Transform projectile, float time, float stick)
	{
		projectile.gameObject.SetActive(true);
		if (trail)
		{
			trail.gameObject.SetActive(true);
			tailEnd.position = projectile.position;
		}
		if (time <= 0) time = 0.1f;
		// orient towards target

		projectile.up = goal.position - projectile.position;

		Vector3 start = projectile.position;

		for (float t = 0; t < time; t += Time.deltaTime)
		{
			if (t > stick) tailEnd.position = Vector3.Lerp(start, goal.position, (t - stick) / time);
			projectile.position = Vector3.Lerp(start, goal.position, t / time);
			yield return new WaitForSeconds(0f);
		}
		projectile.transform.position = goal.position;

		if (stick > time)
		{
			yield return new WaitForSeconds(stick - time);
			stick -= time;
		}
		start = tailEnd.position;

		if(trail && stick > 0f)
			for (float t = 0f; t < stick; t += Time.deltaTime)
			{
				tailEnd.position = Vector3.Lerp(start, projectile.position, t / stick);
				yield return new WaitForSeconds(0f);
			}
		else if(stick > 0f)
			yield return new WaitForSeconds(stick);
		
		projectile.gameObject.SetActive(false);
		if (trail) trail.gameObject.SetActive(false);
	}
}
