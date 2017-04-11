using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float travelingTime = 0.1f;
	public float stick = 0.3f;
	public bool distanceFactor = true;

	public GameObject prefab;
	public StrechBetweenTwoObjects trailPrefab;

	Transform projectile;
	StrechBetweenTwoObjects trail;
	Transform tailEnd;
	Transform dummy;

	public LocationEvent EventOnImpact = new LocationEvent();

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
		dummy = new GameObject("dummy").transform;
	}

	public void FireAt(GameObject o)
	{
		projectile.transform.position = transform.position;
		float time = (distanceFactor) ? travelingTime * (o.transform.position - transform.position).magnitude : travelingTime;
		StartCoroutine(Animate(o.transform, projectile, time, stick));
	}

	public void FirePast(GameObject o)
	{
		projectile.transform.position = transform.position;
		dummy.position = o.transform.position + (o.transform.position - transform.position) * 0.75f + new Vector3(Random.value, Random.value);
		float time = (distanceFactor) ? travelingTime * (o.transform.position - transform.position).magnitude : travelingTime;
		StartCoroutine(Animate(dummy, projectile, time, stick));
	}

	public void FireAt(Component c)
	{
		projectile.transform.position = transform.position;
		float time = (distanceFactor) ? travelingTime * (c.transform.position - transform.position).magnitude : travelingTime;
		StartCoroutine(Animate(c.transform, projectile, time, stick));
	}
	public void FirePast(Component c)
	{
		projectile.transform.position = transform.position;
		dummy.position = c.transform.position + (c.transform.position - transform.position) * 0.75f + new Vector3(Random.value, Random.value);
		float time = (distanceFactor) ? travelingTime * (c.transform.position - transform.position).magnitude : travelingTime;
		StartCoroutine(Animate(dummy, projectile, time, stick));
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
			if (t > stick && trail) tailEnd.position = Vector3.Lerp(start, goal.position, (t - stick) / time);
			if (!goal) break;
			projectile.position = Vector3.Lerp(start, goal.position, t / time);
			yield return new WaitForSeconds(0f);
		}
		if (goal)
		{
			projectile.transform.position = goal.position;
			EventOnImpact.Invoke(projectile.transform.position);

			if (stick > time)
			{
				yield return new WaitForSeconds(stick - time);
				stick -= time;
			}
			if (trail) start = tailEnd.position;

			if (trail && stick > 0f)
				for (float t = 0f; t < stick; t += Time.deltaTime)
				{
					tailEnd.position = Vector3.Lerp(start, projectile.position, t / stick);
					yield return new WaitForSeconds(0f);
				}
			else if (stick > 0f)
				yield return new WaitForSeconds(stick);
		}
		
		projectile.gameObject.SetActive(false);
		if (trail) trail.gameObject.SetActive(false);
	}

	void OnApplicationQuit()
	{
		teardown = true;
	}
	bool teardown = false;
	void OnDestroy()
	{
		if (teardown) return;
		Destroy(projectile.gameObject);
		if(trail) Destroy(trail.gameObject);
		if(tailEnd)Destroy(tailEnd.gameObject);
		Destroy(dummy.gameObject);
	}

	[System.Serializable]
	public class LocationEvent : UnityEvent<Vector2> { }
}
