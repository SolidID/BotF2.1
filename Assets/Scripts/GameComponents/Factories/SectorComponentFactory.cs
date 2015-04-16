using System;
using Assets.Scripts.API;
using Assets.Scripts.Configuration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameComponents.Factories
{
	public static class SectorComponentFactory
	{
		public static SectorComponent Create(ISector model, GameObject row, int size)
		{
			var sectorHolder = new GameObject("SectorWrapper " + model.Coordinates.x);
			sectorHolder.AddComponent<SectorComponent>();
			var sector = sectorHolder.GetComponent<SectorComponent>();
			sector.Model = model;
			sector.transform.position = ToPixel(model.Coordinates, size);
			GenerateSystem(sector);
			sector.InitializeComponents();

			sectorHolder.transform.parent = row.transform;
			return sector;
		}

		private static Vector3 ToPixel(Vector2 hexCoords, int size)
		{
			float correctedX = hexCoords.x + 1;
			float correctedY = hexCoords.y + 1;

			float x = (correctedX * Globals.Width) - (((int)correctedY & 1) * Globals.HalfWidth) - (GameSettings.Instance.GalaxySize * Globals.Width + Globals.HalfWidth) / 2f;
			float z = (correctedY * Globals.Radius + (correctedY - 1) * (Globals.Radius / 2)) - ((GameSettings.Instance.GalaxySize - 1) * 1.5f * Globals.Radius + Globals.Height) / 2f;

			return new Vector3(x, 0, z);
		}

		private static void GenerateSystem(SectorComponent sector)
		{
			if (sector.Model.Planets.Count == 0)
				return;

			Vector3 randomPosition = Vector3.Scale(Random.insideUnitSphere * Globals.Radius / 3, new Vector3(1, 0, 1));

			GameObject sun = MonoBehaviour.Instantiate(Resources.Load("Prefabs/SunPrefab"), randomPosition, Quaternion.identity) as GameObject;
			float sunSize = Random.Range(.1f, .4f);
			sun.transform.localScale = new Vector3(sunSize, sunSize, sunSize);
			sun.transform.parent = sector.gameObject.transform;
			sun.transform.rotation = new Quaternion(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
			sun.transform.localPosition = randomPosition + new Vector3(0, 0, 0);
			var sunColor = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1));

			sun.transform.FindChild("Sun").GetComponent<Renderer>().material.SetColor("_Color", sunColor);
			sun.transform.FindChild("Sun").GetComponent<Renderer>().material.SetColor("_EmissionColor", sunColor);
			sun.transform.FindChild("Sun").GetComponent<Renderer>().material.SetColor("_EmissionColorUI", sunColor);
			sun.transform.FindChild("Sun").GetComponent<Renderer>().material.SetFloat("_EmissionScaleUI", 3.55f);
			sun.transform.FindChild("FwdRadiation").GetComponent<Renderer>().material.SetColor("_DiffuseColorAdjustment", sunColor - (sunColor * new Color(0.5f, 0.5f, 0.5f)));
			sun.transform.FindChild("FwdRadiation").GetComponent<Renderer>().material.SetColor("_Color", sunColor);
			sun.transform.FindChild("BwdRadiation").GetComponent<Renderer>().material.SetColor("_DiffuseColorAdjustment", sunColor - (sunColor * new Color(0.5f, 0.5f, 0.5f)));
			sun.transform.FindChild("BwdRadiation").GetComponent<Renderer>().material.SetColor("_Color", sunColor);

			sector.Sun = sun;
		}

		// TODO extract PlanetComponent + Factory
		//private static void CreatePlanets(float sunSize, GameObject sun)
		//{
		//	int countPlanets = Random.Range(0, 12);
		//	Planets = new List<GameObject>(countPlanets);
		//	for (int i = 0; i < countPlanets; i++)
		//	{
		//		Vector3 randomPlanetPosition = Vector3.Scale(Random.insideUnitSphere * sunSize, new Vector3(1, 0, 1));
		//		GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//		float planetSize = Random.Range(0.33f, sunSize / 2);
		//		planet.gameObject.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
		//		planet.gameObject.transform.parent = sun.transform;
		//		planet.gameObject.transform.localPosition = randomPlanetPosition;

		//		var heightmap = HorizontalTransientDiamondSquare.Create(new HorizontalTransientDiamondSquare.Dto()
		//		{
		//			Width = 512,
		//			Height = 512,
		//			Seed = Random.Range(0, Int32.MaxValue),
		//			Roughness = 0.8,
		//			StartType = DiamondSquareInitType.Random
		//		}, PostProcess);
		//		var mat = new Material(Shader.Find("Tessellation/Bumped Specular (displacement)"));
		//		var textureMap = Resources.Load<Texture2D>("Textures/terrains/terran");
		//		var text = TextureFactory.Create(heightmap, textureMap);
		//		heightmap = AdjustContrast(heightmap, 18);
		//		var nrml = TextureFactory.CreateNormalMapFromHeightMap(GetHeightData(heightmap), 100);
		//		mat.SetTexture("_MainTex", text);
		//		mat.SetTexture("_BumpMap", nrml);
		//		mat.SetTexture("_ParallaxMap", nrml);
		//		mat.SetFloat("_Parallax", 0.11225f);
		//		mat.SetFloat("_Shininess", 1f);
		//		mat.SetColor("_SpecColor", Color.black);
		//		mat.SetFloat("_EdgeLength", 3f);

		//		planet.gameObject.transform.GetComponent<Renderer>().material = mat;
		//		Planets.Add(planet);
		//	}

		//	_planetVisible = true;
		//}
	}
}
