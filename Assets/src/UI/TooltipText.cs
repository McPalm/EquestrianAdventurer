using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TooltipText : MonoBehaviour
{
	[SerializeField]
	Text text;
	[SerializeField]
	RectTransform background;
	static TooltipText _instance;
	bool left;
	Vector2 size;
	public float padding = 7f;

	public static TooltipText Instance
	{
		get
		{
			return _instance;
		}
	}

	Vector2 Size
	{
		set
		{
			if(size != value)
			{
				size = value;
				background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
				background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

			}
		}

	}


	// Use this for initialization
	void Awake()
	{
		_instance = this;
		gameObject.SetActive(false);
	}

	public void Display(string hint)
	{
		if (Input.GetMouseButton(0)) return;

		text.text = hint;
		gameObject.SetActive(true);
		Place();
	}

	public void Hide(string hint)
	{
		if (hint == text.text) gameObject.SetActive(false);
	}

	public void Update()
	{
		if (Input.GetMouseButton(0)) gameObject.SetActive(false);
		Place();
	}

	void Place()
	{
		Size = new Vector2(text.preferredWidth + padding * 2f, text.preferredHeight + padding * 2f);
		if (left && Input.mousePosition.x < size.x) left = false;
		else if(!left && Input.mousePosition.x > Screen.width - size.x) left = true;
		if (left)
		{
			// text.transform.localPosition *= -1f;
			background.localPosition = new Vector3(-size.x / 2f - padding * 2f, 0f);
		}
		else
		{
			//text.transform.localPosition *= -1f;
			background.localPosition = new Vector3(size.x / 2f + padding * 2f, 0f);
		}

		transform.position = Input.mousePosition;
	}
}
