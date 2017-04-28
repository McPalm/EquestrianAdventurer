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

	OvermapData.SectionGroupData selectedGroup;
	IntVector2 selectedTile;

	// Use this for initialization
	void Start ()
	{
		editMode = SelectMode;
		//model = new OvermapData();
		Load();

		model.EventEditGroup.AddListener(OnEditGroup);
		model.EventEditSection.AddListener(OnEditSection);
		model.EventRemoveSection.AddListener(OnRemoveSection);
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
			selectedTile = iv2;
			sectionView.Show(model, iv2);
			selectedGroup = model.GroupOf(iv2);
			if (selectedGroup != null)
			{
				foreach (IntVector2 member in selectedGroup.members)
				{
					GetOrBuildAt(member).FocusGroup = true;
				}
				groupView.Show(model, selectedGroup);
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
			selectedGroup = null;
			selectedTile = IntVector2.MaxValue;
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

		render.Module = section.module.Length > 0;
		render.Custom = !(section.inheritGenerator && section.inheritSpawnTable);
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

	void OnRemoveSection(IntVector2 location, OvermapData.SectionContainer section)
	{
		SectionMapIcon tile = null;
		if(tiles.TryGetValue(location, out tile))
		{
			Destroy(tile.gameObject);
			tiles.Remove(location);
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
		select();
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

	void AddMode()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

		if (Input.GetMouseButtonDown(0))
		{
			IntVector2 location = IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (model.sections.ContainsKey(location))
			{
				
			}
			else
			{
				model.AddSection(location);
			}

			
			if (selectedGroup != null)
			{
				model.AddSectionToGroup(selectedGroup, location);
				SelectAt(location);
			}
			else if(selectedTile != IntVector2.MaxValue)
			{
				model.AddGroup(selectedTile, location);
				SelectAt(location);
			}
			
			
			return;
		}
	}

	void DeleteMode()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

		if (Input.GetMouseButtonDown(0))
		{
			IntVector2 location = IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			model.RemoveAt(location);
		}
	}

	void select()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			editMode = SelectMode;
			print("Select Mode");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			editMode = AddMode;
			print("Add Mode");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			editMode = DeleteMode;
			print("Delete Mode");
		}
	}

	void Load()
	{
		try
		{
			model = XmlTool.LoadFromXML<OvermapData>("XML/WorldMap");	
		}
		catch
		{
			model = new OvermapData();
			Debug.LogError("Unable to load map, making a new one. Be careful with overwriting any old one.");
		}
		if (model == null)
		{
			model = new OvermapData();
			Debug.LogError("Unable to load map, making a new one. Be careful with overwriting any old one.");
		}
		RefreshView();
	}

	public void Save()
	{
		XmlTool.EditorSaveObjectAsXML(model, "XML/WorldMap");
	}
}

