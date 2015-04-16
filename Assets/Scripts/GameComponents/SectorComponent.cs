using Assets.Scripts.API;
using Assets.Scripts.API.Observers;
using Assets.Scripts.Configuration;
using Assets.Scripts.Debug;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameComponents.Input;
using Assets.Scripts.GameComponents.Meshes;
using BotF2.TerrainGeneration.DiamondSquare;
using BotF2.TerrainGeneration.Texture;
using System;
using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameComponents
{
	public class SectorComponent : MonoBehaviour, IEndTurnObserver, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		public SectorMesh SectorMesh { get; set; }
		public Material Material { get; set; }
		public bool Selected { get; private set; }
		public List<GameObject> Planets { get; set; }
		public GameObject Sun { get; set; }
		public ISector Model { get; set; }

		private bool IsSystem { get { return Sun != null; } }

		private GameObject _sectorNameHolder;
		private readonly Vector3 _textOffset = new Vector3(0, 0, -1);
		private bool _sunVisible;
		private bool _planetVisible;

		public void InitializeComponents()
		{
			var sector = new GameObject("SectorComponent" + Model.Coordinates);

			SectorMesh = sector.AddComponent<SectorMesh>();
			sector.transform.parent = transform;
			sector.transform.localPosition = new Vector3(0, 0, 0);
			sector.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/sector_grid");

			if (IsSystem)
			{
				_sectorNameHolder = new GameObject("sector name holder");
				_sectorNameHolder.AddComponent<GUIText>();
				_sectorNameHolder.GetComponent<GUIText>().text = Model.Name;
				_sectorNameHolder.GetComponent<GUIText>().anchor = TextAnchor.UpperCenter;
				_sectorNameHolder.GetComponent<GUIText>().fontStyle = FontStyle.BoldAndItalic;
				_sectorNameHolder.transform.parent = transform;
			}
		}

		void Update()
		{
			CheckSunVisibility();

			if (Selected)
			{
				DebugOutput.Instance.AddMessage("Selected Sector: {0}".FormatWith(Model.Name));
				//if (Planets != null && Planets.Count > 0)
				//	foreach (var planet in Planets)
				//	{
				//		float currentAngle = planet.transform.rotation.eulerAngles.y;
				//		planet.transform.rotation = Quaternion.AngleAxis(currentAngle + (10 * Time.deltaTime), Vector3.up);
				//	}
			}

			// text positioning
			if (!IsSystem)
				return;

			Vector3 viewPortPoint = Camera.main.WorldToViewportPoint(Sun.transform.position + _textOffset);
			if (viewPortPoint.x > 0 && viewPortPoint.x < 1 && viewPortPoint.y > 0 && viewPortPoint.y < 1)
			{
				float ratio = Screen.width / (float)Screen.height;
				var pixelPerUnit = Screen.width / (2 * Camera.main.orthographicSize * ratio);
				// checking if the size of the text is smaller than the hex field
				if (
						SectorMesh.GetComponent<Renderer>().bounds.size.x * pixelPerUnit > _sectorNameHolder.GetComponent<GUIText>().GetScreenRect().width
						|| CameraController.Instance.Distance <= 75
				   )
				{
					_sectorNameHolder.SetActive(true);
					_sectorNameHolder.transform.position = viewPortPoint;
				}
				else
				{
					_sectorNameHolder.SetActive(false);
				}
			}
			else
			{
				_sectorNameHolder.SetActive(false);
			}
		}

		private void CheckSunVisibility()
		{
			if (!IsSystem)
				return;

			if (CameraController.Instance.Distance > 250)
			{
				if (!_sunVisible)
					return;

				Sun.SetActive(false);

				_sunVisible = false;
			}
			else
			{
				if (Vector3.Distance(Camera.main.transform.position, transform.position) < 75f)
				{
					if (!_planetVisible)
						CreatePlanets(Sun.transform.localScale.x, Sun);
				}
				else
				{
					DestroyPlanets();
				}

				if (_sunVisible)
					return;

				//Sun.SetActive(true);
				_sunVisible = true;
			}
		}

		private void DestroyPlanets()
		{
			foreach (var planet in Planets)
			{
				Destroy(planet);
			}
			_planetVisible = false;
		}

		void LateUpdate()
		{

		}

		// TODO extract PlanetComponent + Factory
		private void CreatePlanets(float sunSize, GameObject sun)
		{
			int countPlanets = Random.Range(0, 12);
			Planets = new List<GameObject>(countPlanets);
			for (int i = 0; i < countPlanets; i++)
			{
				Vector3 randomPlanetPosition = Vector3.Scale(Random.insideUnitSphere * sunSize, new Vector3(1, 0, 1));
				GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				float planetSize = Random.Range(0.33f, sunSize / 2);
				planet.gameObject.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
				planet.gameObject.transform.parent = sun.transform;
				planet.gameObject.transform.localPosition = randomPlanetPosition;

				var heightmap = HorizontalTransientDiamondSquare.Create(new HorizontalTransientDiamondSquare.Dto()
				{
					Width = 512,
					Height = 512,
					Seed = Random.Range(0, Int32.MaxValue),
					Roughness = 0.8,
					StartType = DiamondSquareInitType.Random
				}, PostProcess);
				var mat = new Material(Shader.Find("Tessellation/Bumped Specular (displacement)"));
				var textureMap = Resources.Load<Texture2D>("Textures/terrains/terran");
				var text = TextureFactory.Create(heightmap, textureMap);
				heightmap = AdjustContrast(heightmap, 18);
				var nrml = TextureFactory.CreateNormalMapFromHeightMap(GetHeightData(heightmap), 100);
				mat.SetTexture("_MainTex", text);
				mat.SetTexture("_BumpMap", nrml);
				mat.SetTexture("_ParallaxMap", nrml);
				mat.SetFloat("_Parallax", 0.11225f);
				mat.SetFloat("_Shininess", 1f);
				mat.SetColor("_SpecColor", Color.black);
				mat.SetFloat("_EdgeLength", 3f);

				planet.gameObject.transform.GetComponent<Renderer>().material = mat;
				Planets.Add(planet);
			}

			_planetVisible = true;
		}

		private static float[,] GetHeightData(Texture2D heightMap)
		{
			float[,] data = new float[heightMap.width, heightMap.height];
			for (int y = 0; y < heightMap.height; y++)
			{
				for (int x = 0; x < heightMap.width; x++)
				{
					data[x, y] = heightMap.GetPixel(x, y).r;
				}
			}
			return data;
		}

		private static float[,] PostProcess(float[,] map)
		{
			float exponent = Random.Range(0.8f, 5f);
			for (int y = 0; y < map.GetLength(1); y++)
			{
				for (int x = 0; x < map.GetLength(0); x++)
				{
					var value = map[x, y];
					map[x, y] = (float)Math.Pow(value, exponent);
				}
			}
			return map;
		}

		private static Texture2D AdjustContrast(Texture2D map, float value)
		{
			float contrast = (float)Math.Pow((100 + value) / 100, 2);

			for (int y = 0; y < map.height; y++)
			{
				for (int x = 0; x < map.width; x++)
				{
					float col = map.GetPixel(x, y).r;
					float adjVal = ((col - 0.5f) * contrast + 0.5f);
					if (adjVal < 1) col = adjVal;
					map.SetPixel(x, y, new Color(col, col, col, 1));
				}
			}
			return map;

		}

		public void EndTurn()
		{
			if (Planets != null && Planets.Count > 0)
				foreach (GameObject planet in Planets)
				{
					planet.gameObject.transform.RotateAround(Sun.gameObject.transform.position, Sun.gameObject.transform.up, Random.Range(-90, 90));
				}
		}

		public void Deselect()
		{
			Selected = false;
			SectorMesh.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/sector_grid");
			if (IsSystem)
				_sectorNameHolder.GetComponent<GUIText>().color = Color.white;
		}

		public override string ToString()
		{
			return Model.Name;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!Selected)
				SectorMesh.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/sector_grid");
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!Selected)
				SectorMesh.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/sector_grid_selected");
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Selected = true;
			SectorMesh.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/sector_grid_selected");
			SectorMesh.gameObject.GetComponent<Renderer>().material.color = GameSettings.Instance.SelectedSectorColor;

			if (IsSystem)
				_sectorNameHolder.GetComponent<GUIText>().color = GameSettings.Instance.SelectedSectorColor;

			SectorSelectionHandler.SelectedSectorComponent = this;
		}
	}
}
