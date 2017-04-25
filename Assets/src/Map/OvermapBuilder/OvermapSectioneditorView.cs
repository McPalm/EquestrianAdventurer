using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OvermapSectioneditorView : MonoBehaviour
{
	[SerializeField]
	Button InheritGeneratorButton;
	[SerializeField]
	GameObject InheritGeneratorEnabledIcon;
	[SerializeField]
	Dropdown GeneratorDropdown;
	[SerializeField]
	InputField PremadeSectionInput;

	[Space(10f)]

	[SerializeField]
	Button InheritSpawntableButton;
	[SerializeField]
	GameObject InheritSpawntableEnabledIcon;
	[SerializeField]
	InputField SpawntableInput;

	[Space(10f)]

	[SerializeField]
	Slider colorSlider;

	[Space(10f)]
	[SerializeField]
	Button NorthConnectionButton;
	[SerializeField]
	GameObject NorthConnectionIcon;
	[SerializeField]
	Button EastConnectionButton;
	[SerializeField]
	GameObject EastConnectionIcon;
	[SerializeField]
	Button SouthConnectionButton;
	[SerializeField]
	GameObject SouthConnectionIcon;
	[SerializeField]
	Button WestConnectionButton;
	[SerializeField]
	GameObject WestConnectionIcon;

	OvermapData data;
	IntVector2 location;

	bool ignoreEvents = false; // Unity UI breaks the VMC contract and acts as if player change the value on a view update. This is a workaround to ignore events during the refresh.

	void Start()
	{
		List<string> options = new List<string>(System.Enum.GetNames(typeof(MapType)));
		GeneratorDropdown.AddOptions(options);
		GeneratorDropdown.onValueChanged.AddListener(OnGeneratorInput);
		PremadeSectionInput.onEndEdit.AddListener(OnPremadeSectionInput);

		SpawntableInput.onEndEdit.AddListener(OnSpawnInput);

		colorSlider.onValueChanged.AddListener(OnColorSlider);

		InheritGeneratorButton.onClick.AddListener(OnInheritGenerator);
		InheritSpawntableButton.onClick.AddListener(OnInheritSpawn);

		NorthConnectionButton.onClick.AddListener(OnNorthConnection);
		EastConnectionButton.onClick.AddListener(OnEastConnection);
		SouthConnectionButton.onClick.AddListener(OnSouthConnection);
		WestConnectionButton.onClick.AddListener(OnWestConnection);
	}

	public void Show(OvermapData data, IntVector2 location)
	{
		if (this.data != null) data.EventEditSection.RemoveListener(OnDataSectionEdit);

		OvermapData.SectionContainer target = null;
		data.sections.TryGetValue(location, out target);

		if (target == null)
		{
			Hide();
			return;
		}

		gameObject.SetActive(true);
		this.data = data;
		this.location = location;

		data.EventEditSection.AddListener(OnDataSectionEdit);
		Refresh(target);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	void OnDataSectionEdit(IntVector2 location, OvermapData.SectionContainer container)
	{
		if (this.location == location)
			Refresh(container);
	}

	void Refresh(OvermapData.SectionContainer section)
	{
		ignoreEvents = true;
		InheritGeneratorEnabledIcon.SetActive(section.inheritGenerator);
		GeneratorDropdown.interactable = !section.inheritGenerator;
		GeneratorDropdown.value = (int)section.generator;
		if (section.generator == MapType.pregenerated)
		{
			PremadeSectionInput.interactable = true;
			PremadeSectionInput.text = section.pregeneratedName;
		}
		else
		{
			PremadeSectionInput.interactable = false;
			PremadeSectionInput.text = "";
		}

		InheritSpawntableEnabledIcon.SetActive(section.inheritSpawnTable);
		SpawntableInput.interactable = !section.inheritSpawnTable;
		SpawntableInput.text = section.spawntable;
		
		SpawntableInput.GetComponent<Image>().color = (CreatureSpawner.HasSpawner(section.spawntable)) ? new Color(0.7f, 1f, 0.7f) : new Color(1f, 0.7f, 0.7f);
	
		NorthConnectionIcon.SetActive((section.connections & CompassDirection.north) == CompassDirection.north);
		EastConnectionIcon.SetActive((section.connections & CompassDirection.east) == CompassDirection.east);
		SouthConnectionIcon.SetActive((section.connections & CompassDirection.south) == CompassDirection.south);
		WestConnectionIcon.SetActive((section.connections & CompassDirection.west) == CompassDirection.west);

		float h, s, v;
		Color.RGBToHSV(section.color, out h, out s, out v);
		colorSlider.value = h;
		colorSlider.handleRect.GetComponent<Image>().color = section.color;

		ignoreEvents = false;
	}




	/////////////////////////
	// Incomming Events from the view
	//

	void OnColorSlider(float f)
	{
		if (ignoreEvents) return;
		data.SetSectionColor(location, Color.HSVToRGB(f, 0.9f, 0.95f));
	}

	void OnSpawnInput(string s)
	{
		if (ignoreEvents) return;
		data.SetSectionSpawntable(location, s);
	}

	void OnGeneratorInput(int i)
	{
		if (ignoreEvents) return;
		data.SetSectionGenerator(location, (MapType)i);
	}

	void OnInheritSpawn()
	{
		if (ignoreEvents) return;
		data.SetSectionSpawnInherit(location, !InheritSpawntableEnabledIcon.activeSelf);
	}

	void OnInheritGenerator()
	{
		if (ignoreEvents) return;
		data.SetSectionGeneratorInherit(location, !InheritGeneratorEnabledIcon.activeSelf);
	}

	void OnPremadeSectionInput(string s)
	{
		if (ignoreEvents) return;
		data.SetSectionPregeneratedName(location, s);
	}

	void OnNorthConnection()
	{
		if (ignoreEvents) return;
		ToggleConnection(CompassDirection.north);
	}

	void OnEastConnection()
	{
		if (ignoreEvents) return;
		ToggleConnection(CompassDirection.east);
	}

	void OnWestConnection()
	{
		if (ignoreEvents) return;
		ToggleConnection(CompassDirection.west);
	}

	void OnSouthConnection()
	{
		if (ignoreEvents) return;
		ToggleConnection(CompassDirection.south);
	}

	void ToggleConnection(CompassDirection direction)
	{
		if (ignoreEvents) return;
		OvermapData.SectionContainer container = null;

		if (data.sections.TryGetValue(location, out container))
		{
			if ((container.connections & direction) == 0)
				data.AddConnection(location, direction);
			else
				data.RemoveConnection(location, direction);
		}
	}
}
