using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuraIcon : MonoBehaviour
{
	public Aura target;

	[SerializeField]
	Text counter;
	[SerializeField]
	Image icon;
	[SerializeField]
	Tooltip tip;

	DurationAura da;

	// Use this for initialization
	void Start ()
	{
		icon.sprite = target.Icon;
		icon.color = target.IconColor;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target)
		{
			counter.text = target.IconText;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
