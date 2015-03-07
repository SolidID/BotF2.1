using System.Collections.Generic;
using Assets.Scripts.API.Observers;
using UnityEngine;

namespace Assets.Scripts.GameComponents.Input
{
	public class KeyboardHandler : MonoBehaviour
	{
		private static KeyboardHandler _instance;
		private readonly List<IEndTurnObserver> _observers;


		public static KeyboardHandler Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<KeyboardHandler>() ?? new GameObject("KeyboardHandler").AddComponent<KeyboardHandler>();
					DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}
		}

		private KeyboardHandler()
		{
			_observers = new List<IEndTurnObserver>();
		}

		void Awake()
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if (this != Instance)
				Destroy(this.gameObject);

		}


		void Start()
		{

		}

		void LateUpdate()
		{
			if (UnityEngine.Input.GetKeyUp(KeyCode.Return) || UnityEngine.Input.GetKeyUp(KeyCode.KeypadEnter))
				FireEndOfTurn();
		}

		public void AddEndTurnObserver(IEndTurnObserver observer)
		{
			if (!_observers.Contains(observer))
				_observers.Add(observer);
		}

		public void RemoveEndTurnObserver(IEndTurnObserver observer)
		{
			_observers.Remove(observer);
		}

		public void FireEndOfTurn()
		{
			foreach (IEndTurnObserver observer in _observers)
			{
				observer.EndTurn();
			}
		}
	}
}
