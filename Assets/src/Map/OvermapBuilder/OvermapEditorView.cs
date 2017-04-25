using UnityEngine;
using System.Collections.Generic;

public class OvermapEditorView : MonoBehaviour
{
	[SerializeField]
	SectionMapIcon sectionIconPrefab;
	[SerializeField]
	OvermapGroupView groupView;
	[SerializeField]
	OvermapSectioneditorView sectionView;


	OvermapData model;
	Dictionary<IntVector2, SectionMapIcon> tiles = new Dictionary<IntVector2, SectionMapIcon>();

	// Use this for initialization
	void Start ()
	{
		// hack together a model for testing

		model = new OvermapData();

		model.AddSection(IntVector2.zero);


		RefreshView();
		model.EventEditSection.AddListener(OnEditSection);
		model.EventEditGroup.AddListener(OnEditGroup);

		model.AddSection(new IntVector2(0, 1));
		model.AddSection(new IntVector2(1, 1));
		model.AddSection(new IntVector2(1, 0));

		model.SetSectionColor(IntVector2.zero, Color.white);

		model.AddGroup(IntVector2.zero, new IntVector2(0, 1), new IntVector2(1, 1));

		groupView.Show(model, model.groups[0]);

		sectionView.Show(model, new IntVector2(1, 1));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void RefreshView()
	{
		foreach(KeyValuePair<IntVector2, OvermapData.SectionContainer> pair in model.sections)
		{
			OnEditSection(pair.Key, pair.Value);
		}
	}

	void OnEditSection(IntVector2 location, OvermapData.SectionContainer section)
	{
		SectionMapIcon render = GetOrBuildAt(location);

		render.Color = section.color;
		render.SetConnections = section.connections;
	}

	void OnEditGroup(OvermapData.SectionGroupData group)
	{
		foreach(IntVector2 location in group.members)
		{
			OvermapData.SectionContainer member;
			if (model.sections.TryGetValue(location, out member))
				OnEditSection(location, member);
		}
	}

	SectionMapIcon GetOrBuildAt(IntVector2 location)
	{
		SectionMapIcon render = null;
		tiles.TryGetValue(location, out render);
		if (render) return render;
		render = Instantiate(sectionIconPrefab, (Vector3)location, Quaternion.identity) as SectionMapIcon;
		tiles.Add(location, render);
		return render;
	}
}
