using System;
using Assets;
using BotF2.TerrainGeneration.DiamondSquare;
using BotF2.TerrainGeneration.Texture;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class NewTexture : MonoBehaviour {

	public GameObject Planet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseEnter()
	{
		gameObject.GetComponent<GUIText>().color = new Color(10 / 255f, 120 / 255f, 174 / 255f);
	}

	void OnMouseExit()
	{
		gameObject.GetComponent<GUIText>().color = Color.white;
	}

	void OnMouseUp()
	{
		LoadRandomPlanetTexture.GeneratePlanet(new LoadRandomPlanetTexture.GeneratePlanetDto(){Planet = Planet, Seed = Random.Range(0, Int32.MaxValue)} );
	}
}
