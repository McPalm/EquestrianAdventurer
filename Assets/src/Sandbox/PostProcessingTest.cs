using UnityEngine;
using System.Collections;

public class PostProcessingTest : MonoBehaviour
{
	public Shader myShader;
	Material material;

	public Color color;

	// Use this for initialization
	void Start ()
	{
		material = new Material(myShader);
	}
	
	// Update is called once per frame
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetColor("_Color", color);
		Graphics.Blit(source, destination, material);
	}
}
