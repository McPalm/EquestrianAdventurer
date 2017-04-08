using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PropogateSprite : MonoBehaviour
{
	public SpriteEvent EventSetSprite = new SpriteEvent();
	public ColorEvent EventSetColor = new ColorEvent();

	public void SetSpriteFrom(Component c)
	{
		SetSpriteFrom(c.gameObject);
	}

	public void SetSpriteFrom(GameObject o)
	{
		SpriteRenderer sr = o.GetComponent<SpriteRenderer>();
		if(sr)
		{
			EventSetSprite.Invoke(sr.sprite);
			EventSetColor.Invoke(sr.color);
		}
	}

	[System.Serializable]
	public class SpriteEvent : UnityEvent<Sprite> { }
	[System.Serializable]
	public class ColorEvent : UnityEvent<Color> { }
}
