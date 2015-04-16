using Assets.Scripts.API;
using Assets.Scripts.API.Observers;
using Assets.Scripts.Configuration;
using Assets.Scripts.GameComponents.Input;
using Assets.Scripts.GameComponents.Meshes;
using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameComponents.Factories;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameComponents
{
	public class Game : MonoBehaviour, IEndTurnObserver
	{
		public static Game Instance { get; private set; }

		public SectorMesh.SectorMeshMode GridStyle = SectorMesh.SectorMeshMode.Flat;
		public GalaxySize GalaxySize = GalaxySize.Normal;
		public float HorizontalMouseSensitivity;
		public float VerticalMouseSensitivity;
		public Color SelectedSectorColor;
		public GUISkin Skin;
		public RectTransform UpperUi;

		private List<Player> _players;
		private int _turnNumber = 1;
		private IGalaxy _galaxyModel;

		public Player CurrentPlayer
		{
			get { return _players[0]; }
		}

		public int TurnNumber { get { return _turnNumber; } }

		public Game()
		{

		}

		void Start()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				throw new InvalidOperationException("There is only on instance of Game allowed!");
			}

			InitializeGameSettings();
			CreatePlayers();

			_galaxyModel = GalaxyFactory.Create(GameSettings.Instance.GalaxySize);
			GalaxyComponentFactory.Model = _galaxyModel;
			GalaxyComponentFactory.Create().ToList();

			KeyboardHandler.Instance.AddEndTurnObserver(this);
			//_galEnum = GalaxyComponentFactory.Create()
			//                                 .GetEnumerator();
		}

		private void CreatePlayers()
		{
			_players = new List<Player>();
			_players.Add(new Player()
			{
				Name = "Föderation",
				Credits = 1000f,
				Income = 123.45f
			});
		}

		void Update()
		{
			//StartCoroutine(_galEnum);
		}

		void LateUpdate()
		{

		}

		public void EndTurn()
		{
			_turnNumber++;
			UpperUi.Find("TurnValue").GetComponent<Text>().text = _turnNumber.ToString("N0");
			UpperUi.Find("CreditsValue").GetComponent<Text>().text = _players[0].Credits.ToString("N0");
		}

		private void InitializeGameSettings()
		{
			GameSettings.Instance.SectorMeshRenderMode = GridStyle;
			GameSettings.Instance.GalaxySize = (int)GalaxySize;
			GameSettings.Instance.HorizontalMouseSensitivity = HorizontalMouseSensitivity;
			GameSettings.Instance.VerticalMouseSensitivity = VerticalMouseSensitivity;
			GameSettings.Instance.SelectedSectorColor = SelectedSectorColor;
		}
	}
}
