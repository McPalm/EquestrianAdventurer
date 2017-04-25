using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
	System.Action editMode;

	// Use this for initialization
	void Start ()
	{
		// hack together a model for testing

		editMode = SelectMode;

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

		/*
		groupView.Show(model, model.groups[0]);

		sectionView.Show(model, new IntVector2(1, 1));
		*/
		SelectAt(new IntVector2(1, 1));
	}
	
	void SelectAt(IntVector2 iv2)
	{
		foreach(SectionMapIcon icon in tiles.Values)
		{
			icon.FocusTile = false;
			icon.FocusGroup = false;
		}

		if(model.sections.ContainsKey(iv2))
		{
			// select thingy
			sectionView.Show(model, iv2);
			OvermapData.SectionGroupData group = model.GroupOf(iv2);
			if (group != null)
			{
				foreach (IntVector2 member in group.members)
				{
					GetOrBuildAt(member).FocusGroup = true;
				}
				groupView.Show(model, group);
			}
			else
				groupView.Hide();

			GetOrBuildAt(iv2).FocusTile = true;
		}
		else
		{
			// deselect
			groupView.Hide();
			sectionView.Hide();
		}
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



	void Update()
	{
		editMode();
	}

	void SelectMode()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

		if(Input.GetMouseButtonDown(0))
		{
			SelectAt(IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
			return;
		}
	}
}
