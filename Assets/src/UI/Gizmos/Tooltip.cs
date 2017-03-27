using UnityEngine;
using UnityEngine.EventSystems;


public class Tooltip : EventTrigger
{
	[SerializeField]
	public string hint;
	
	public override void OnPointerEnter(PointerEventData data)
	{
		TooltipText.Instance.Display(hint);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		TooltipText.Instance.Hide(hint);
	}

	void OnDisable()
	{
		if(TooltipText.Instance) TooltipText.Instance.Hide(hint);
	}
}
