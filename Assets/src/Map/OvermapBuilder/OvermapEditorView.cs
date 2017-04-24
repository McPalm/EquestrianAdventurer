﻿using UnityEngine;
using System.Collections.Generic;

public class OvermapEditorView : MonoBehaviour
{
	[SerializeField]
	SpriteRenderer sectionIconPrefab;


	OvermapData model;
	Dictionary<IntVector2, SpriteRenderer> tiles = new Dictionary<IntVector2, SpriteRenderer>();

	// Use this for initialization
	void Start ()
	{
		// hack together a model for testing

		model = new OvermapData();

		model.AddSection(new IntVector2(0, 0));


		RefreshView();
		model.EventEditSection.AddListener(OnEditSection);
		model.EventEditGroup.AddListener(OnEditGroup);

		model.AddSection(new IntVector2(0, 1));
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
		SpriteRenderer render = GetOrBuildAt(location);
		render.color = section.color;
	}

	void OnEditGroup(OvermapData.SectionGroupData group)
	{

	}

	SpriteRenderer GetOrBuildAt(IntVector2 location)
	{
		SpriteRenderer render = null;
		tiles.TryGetValue(location, out render);
		if (render) return render;
		render = Instantiate(sectionIconPrefab, (Vector3)location, Quaternion.identity) as SpriteRenderer;
		return render;
	}
}
