using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityUI : MonoBehaviour
{
	[SerializeField]
	Button[] abilityButtons;
	Image[] AbilityIcons;
	AActiveAbility[] abilitites;

	[SerializeField]
	RogueController character;
	[SerializeField]
	Transform selectedMarker;


	// Use this for initialization
	void Start ()
	{
		AbilityIcons = new Image[abilityButtons.Length];
		abilitites = new AActiveAbility[abilityButtons.Length];
		for (int i = 0; i < abilityButtons.Length; i++)
		{
			foreach(Image image in abilityButtons[i].GetComponentsInChildren<Image>())
			{
				if (image.gameObject == abilityButtons[i].gameObject) continue;
				AbilityIcons[i] = image;
			}
			abilitites[i] = null;
			AbilityIcons[i].enabled = false;
		}

		Build();
	}
	

	void Build()
	{
		AActiveAbility[] a = character.Abilities;
		for (int i = 0; i < a.Length; i++)
		{
			SetButton(i, a[i]);
		}
		SelectAbility(0);
	}

	void SetButton(int i, AActiveAbility a)
	{
		AbilityIcons[i].sprite = a.Icon;
		AbilityIcons[i].enabled = true;
		abilitites[i] = a;
		abilityButtons[i].GetComponent<Tooltip>().hint = a.AbilityName + "\n" + a.Description;
	}

	public void SelectAbility(int i)
	{
		if(abilitites[i] != null)
		{
			character.SelectedAbility = abilitites[i];
			selectedMarker.SetParent(abilityButtons[i].transform);
			selectedMarker.localPosition = Vector3.zero;
			// selectedMarker.SetAsFirstSibling();
		}
	}
}
