using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickupButton : MonoBehaviour
{
	MapObject cha;
	CharacterActionController con;

	[SerializeField]
	Button button;
	[SerializeField]
	Image icon;
	[SerializeField]
	Tooltip tooltip;

	MapObject groundObject;

	void Start()
	{
		RogueController rc = FindObjectOfType<RogueController>();
		cha = rc.GetComponent<MapObject>();
		con = rc.GetComponent<CharacterActionController>();

		button.onClick.AddListener(Pickup);
		con.EventAfterAction.AddListener(OnMove);

		OnMove(null, CharacterActionController.Actions.idle);
	}

	void OnMove(CharacterActionController c, CharacterActionController.Actions a)
	{
		groundObject = null;
		foreach (MapObject o in ObjectMap.Instance.ObjectsAtLocation(cha.RealLocation))
		{
			if(o != cha && o.GetComponent<GroundItem>())
			{
				groundObject = o;
				break;
			}
		}
		if(groundObject)
		{
			button.gameObject.SetActive(true);
			SpriteRenderer s = groundObject.GetComponent<SpriteRenderer>();
			icon.sprite = s.sprite;
			icon.color = s.color;
			tooltip.hint = groundObject.GetComponent<GroundItem>().item.Tooltip;
		}
		else
		{
			button.gameObject.SetActive(false);
		}
	}

	void Pickup()
	{
		con.StackAction(CharacterActionController.Actions.pickup);
	}
}
