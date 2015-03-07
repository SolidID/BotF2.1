using Assets.Scripts.API.Observers;
using Assets.Scripts.GameComponents.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameComponents.GameLogic
{
	public class IsEndTurnObserver : MonoBehaviour, IEndTurnObserver
	{

		public UnityEvent OnEndTurn;

		// Use this for initialization
		void Start () {
			if(OnEndTurn != null)
				KeyboardHandler.Instance.AddEndTurnObserver(this);
		}
	
		// Update is called once per frame
		void Update () {
	
		}

		public void EndTurn()
		{
			if(OnEndTurn != null)
				OnEndTurn.Invoke();
		}
	}
}
