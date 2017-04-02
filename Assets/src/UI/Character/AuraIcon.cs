using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuraIcon : MonoBehaviour
{
	Aura target;

	[SerializeField]
	Text counter;
	[SerializeField]
	Image icon;
	[SerializeField]
	Tooltip tip;

	DurationAura da;

	// Use this for initialization
	public void SetTarget(Aura target)
	{
		this.target = target;
		icon.sprite = target.Icon;
		icon.color = target.IconColor;
		tip.hint = target.Tooltip;
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
			AuraIconManager.Instance.Deactivate(this);
		}
	}
}
