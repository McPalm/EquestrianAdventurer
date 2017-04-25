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

		NorthConnectionIcon.SetActive((section.connections & CompassDirection.north) == CompassDirection.north);
		EastConnectionIcon.SetActive((section.connections & CompassDirection.east) == CompassDirection.east);
		SouthConnectionIcon.SetActive((section.connections & CompassDirection.south) == CompassDirection.south);
		WestConnectionIcon.SetActive((section.connections & CompassDirection.west) == CompassDirection.west);
	}

	void OnColorSlider(float f)
	{
		data.SetSectionColor(location, Color.HSVToRGB(f, 0.9f, 0.95f));
	}

	void OnSpawnInput(string s)
	{
		data.SetSectionSpawntable(location, s);
	}

	void OnGeneratorInput(int i)
	{
		data.SetSectionGenerator(location, (MapType)i);
	}

	void OnInheritSpawn()
	{
		data.SetSectionSpawnInherit(location, !InheritSpawntableEnabledIcon.activeSelf);
	}

	void OnInheritGenerator()
	{
		data.SetSectionGeneratorInherit(location, !InheritGeneratorEnabledIcon.activeSelf);
	}

	void OnPremadeSectionInput(string s)
	{
		data.SetSectionPregeneratedName(location, s);
	}

	void OnNorthConnection()
	{
		ToggleConnection(CompassDirection.north);
	}

	void OnEastConnection()
	{
		ToggleConnection(CompassDirection.east);
	}

	void OnWestConnection()
	{
		ToggleConnection(CompassDirection.west);
	}

	void OnSouthConnection()
	{
		ToggleConnection(CompassDirection.south);
	}

	void ToggleConnection(CompassDirection direction)
	{
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
