using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SpawnObjectAt : MonoBehaviour
{
	public int scatter = 3;

	public GameObject[] Prefabs;

	public GameObjectEvent EventOnSpawn = new GameObjectEvent();

	public void Spawn(GameObject location)
	{
		MapObject o = location.GetComponent<MapObject>();
		if (o) SpawnAt(o.RealLocation);
		else SpawnAt(IntVector2.RoundFrom(location.transform.position));
	}

	public void Spawn(MapObject location)
	{
		SpawnAt(location.RealLocation);
	}

	public void Spawn(Component location)
	{
		MapObject o = location.GetComponent<MapObject>();
		if (o) SpawnAt(o.RealLocation);
		else SpawnAt(IntVector2.RoundFrom(location.transform.position));
	}

	public void SpawnAt(IntVector2 location)
	{
		EventOnSpawn.Invoke(Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], (Vector3)location, Quaternion.identity) as GameObject);
	}

	public void CrookedSpawn(GameObject location)
	{
		MapObject o = location.GetComponent<MapObject>();
		if (o) CrookedSpawn(o.RealLocation);
		else CrookedSpawn(IntVector2.RoundFrom(location.transform.position));
	}

	public void CrookedSpawn(MapObject location)
	{
		CrookedSpawn(location.RealLocation);
	}

	public void CrookedSpawn(Component location)
	{
		MapObject o = location.GetComponent<MapObject>();
		if (o) CrookedSpawn(o.RealLocation);
		else CrookedSpawn(IntVector2.RoundFrom(location.transform.position));
	}

	public void CrookedSpawn(IntVector2 location)
	{
		int dx = Random.Range(-scatter, scatter + 1);
		int dy = Random.Range(-scatter, scatter + 1);
		if(dx == 0 && dy == 0)
		{
			dx = Random.Range(0, 2);
			dy = Random.Range(0, 2);
			if (dx == 0) dx--;
			if (dy == 0) dy--;
		}
		IntVector2 suggested = new IntVector2(dx, dy);
		int magnitude = location.MagnitudePF;
		if (magnitude > scatter)
		{
			if(Mathf.Abs(dx) > Mathf.Abs(dy))
			{
				// move X closer to centre
				if (dx < 0) suggested.x += scatter;
				else suggested.x -= scatter;
			}
			else
			{
				// move Y closer to centre
				if (dy < 0) suggested.y += scatter;
				else suggested.y -= scatter;
			}
		}

		SpawnAt(location + suggested);
	}

	[System.Serializable]
	public class GameObjectEvent : UnityEvent<GameObject> { }
}
