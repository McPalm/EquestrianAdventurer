using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OvermapGroupView : MonoBehaviour
{
	public Slider hue;
	public InputField groupName;
	public Image colorDisplay;
	public Dropdown generator;
	public InputField spawner;
	public Image spawnNameValidator;
	public InputField moduleCount;
	public GroupModulesUI modules;
	

	OvermapData data;
	OvermapData.SectionGroupData group;
	bool ignoreEvents = false; // Unity UI breaks the VMC contract and acts as if player change the value on a view update. This is a workaround to ignore events during the refresh.

	void Start()
	{
		List<string> options = new List<string>(System.Enum.GetNames(typeof(MapType)));
		generator.AddOptions(options);
		generator.onValueChanged.AddListener(OnDropdown);

		spawner.onEndEdit.AddListener(OnSpawnerEdit);

		hue.onValueChanged.AddListener(OnHueSlider);

		groupName.onEndEdit.AddListener(OnChangeName);

		moduleCount.onEndEdit.AddListener(OnModuleCount);
		modules.EventChange.AddListener(OnChangeModules);
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
		ignoreEvents = true;

		groupName.text = group.groupName;

		float h;
		float s;
		float v;
		Color.RGBToHSV(group.color, out h, out s, out v);
		hue.value = h;

		colorDisplay.color = group.color;

		generator.value = (int)group.generator;

		spawner.text = group.spawntable;
		spawnNameValidator.color = (CreatureSpawner.HasSpawner(group.spawntable)) ? new Color(0.7f, 1f, 0.7f) : new Color(1f, 0.7f, 0.7f);


		moduleCount.text = group.moduleCount.ToString();
		modules.Build(group.modules);

		ignoreEvents = false;
	}


	void OnChangeName(string n)
	{
		data.SetGroupName(group, n);
	}

	void OnDropdown(int i)
	{
		// print(((MapType)i).ToString());
		if (ignoreEvents) return;
		data.SetGroupGenerator(group, (MapType)i);
	}

	void OnSpawnerEdit(string s)
	{
		if (ignoreEvents) return;

		data.SetGroupSpawner(group, s);
	}

	void OnHueSlider(float f)
	{
		if (ignoreEvents) return;
		data.SetGroupColor(group, Color.HSVToRGB(f, 0.5f, 0.95f));
	}

	void OnChangeModules(string[] modules)
	{
		data.SetGroupModules(group, modules);
	}

	void OnModuleCount(string number)
	{
		data.SetGroupModuleCount(group, int.Parse(number));
	}
}
