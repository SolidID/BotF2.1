using System;
using Assets.Scripts.API.Observers;
using Assets.Scripts.Debug;
using Assets.Scripts.GameComponents.Input;

namespace Assets.Scripts.GameComponents
{
    public class Player : IEndTurnObserver
    {
        public float Credits { get; set; }
        public float Income { get; set; }
        public string Name { get; set; }

        public Player()
        {
            KeyboardHandler.Instance.AddEndTurnObserver(this);
        }

        public void EndTurn()
        {
            Credits += Income;
        }
    }
}