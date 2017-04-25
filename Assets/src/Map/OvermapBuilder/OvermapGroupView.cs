using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OvermapGroupView : MonoBehaviour
{
	[SerializeField]
	Slider hue;
	[SerializeField]
	Image colorDisplay;
	[SerializeField]
	Dropdown generator;
	[SerializeField]
	InputField spawner;
	[SerializeField]
	Image spawnNameValidator;
	


	OvermapData data;
	OvermapData.SectionGroupData group;

	void Start()
	{
		List<string> options = new List<string>(System.Enum.GetNames(typeof(MapType)));
		generator.AddOptions(options);
		generator.onValueChanged.AddListener(OnDropdown);

		spawner.onEndEdit.AddListener(OnSpawnerEdit);

		hue.onValueChanged.AddListener(OnHueSlider);
	}

	public void Show(OvermapData data, OvermapData.SectionGroupData group)
	{
		if(this.data != null) this.data.EventEditGroup.RemoveListener(OnChange);

		this.group = group;
		this.data = data;

		gameObject.SetActive(true);
		data.EventEditGroup.AddListener(OnChange);

		Refresh();
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void OnChange(OvermapData.SectionGroupData group)
	{
		if (this.group == group)
			Refresh();
	}

	public void Refresh()
	{
		float h;
		float s;
		float v;
		Color.RGBToHSV(group.color, out h, out s, out v);
		hue.value = h;

		colorDisplay.color = group.color;

		generator.value = (int)group.generator;

		spawner.text = group.spawntable;

		
	}

	void OnDropdown(int i)
	{
		// print(((MapType)i).ToString());

		data.SetGroupGenerator(group, (MapType)i);
	}

	void OnSpawnerEdit(string s)
	{
		if (CreatureSpawner.Get(s) == null)
			spawnNameValidator.color = new Color(1f, 0.7f, 0.7f);
		else
			spawnNameValidator.color = new Color(0.7f, 1f, 0.7f);

		data.SetGroupSpawner(group, s);
	}

	void OnHueSlider(float f)
	{
		data.SetGroupColor(group, Color.HSVToRGB(f, 0.5f, 0.95f));
	}
}
