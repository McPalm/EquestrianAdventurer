using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OvermapGroupView : MonoBehaviour
{
	[SerializeField]
	Slider hue;
	[SerializeField]
	Dropdown generator;
	[SerializeField]
	InputField spawner;


	OvermapData data;
	OvermapData.SectionGroupData group;

	void Start()
	{
		List<string> options = new List<string>(System.Enum.GetNames(typeof(MapType)));
		generator.AddOptions(options);
		generator.onValueChanged.AddListener(OnDropdown);

		spawner.onEndEdit.AddListener(OnSpawnerEdit);
	}

	void Show(OvermapData data, OvermapData.SectionGroupData group)
	{
		this.data.EventEditGroup.RemoveListener(OnChange);

		this.group = group;
		this.data = data;

		gameObject.SetActive(true);
		data.EventEditGroup.AddListener(OnChange);	

		Refresh();
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

		generator.value = (int)group.generator;

		spawner.text = group.spawntable;
	}

	void OnDropdown(int i)
	{
		print(((MapType)i).ToString());
	}

	void OnSpawnerEdit(string s)
	{
		print(s);
	}
}
