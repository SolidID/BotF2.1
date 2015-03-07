using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameComponents;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObjectEvents AddEvents(this GameObject obj)
        {
            obj.AddComponent<GameObjectEvents>();
            return obj.GetComponent<GameObjectEvents>();
        }
    }
}
