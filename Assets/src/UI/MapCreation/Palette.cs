using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Palette : MonoBehaviour
{
	public float buttonDistance;
	public Button prototype;
	public TileDB tileDB;

	int[] swatches;
	MapBuildController controller;

	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<MapBuildController>();
		SetupButtons(controller.palette);
	}

	void SetupButtons(string p)
	{
		tileDB = TileDB.LoadPalette(p);
		swatches = new int[tileDB.tiles.Length];
		//bool flip;
		Color c;

		for (int i = 1; i < swatches.Length; i++)
		{
			swatches[i] = i;

			int n = i;
			Button b = Instantiate(prototype);
			b.GetComponent<Image>().sprite = tileDB.GetSprite(i, out c); // GetTile(swatches[i], out flip);
			b.GetComponent<Image>().color = c;

			b.onClick.AddListener(delegate { pressButton(swatches[n]); });
			b.transform.SetParent(prototype.transform.parent);
			b.transform.position = prototype.transform.position + new Vector3(i * buttonDistance, 0f);
			// b.transform.localScale = new Vector3((flip) ? -1 : 1f, 1f, 1f);
		}
		prototype.onClick.AddListener(delegate { pressButton(swatches[0]); });
		prototype.GetComponent<Image>().sprite = tileDB.GetSprite(0, out c); //TileDB.GetTile(swatches[0], out flip);
		prototype.GetComponent<Image>().color = c;
		//prototype.transform.localScale = new Vector3((flip) ? -1 : 1f, 1f, 1f);
	}

	void pressButton(int i)
	{
		controller.SetSwatch(i);
	}
}
