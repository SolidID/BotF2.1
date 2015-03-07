using System.Collections.Generic;
using Assets.Scripts.API;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Sector : ISector
    {
        public string Name { get; set; }
        public IList<IPlanet> Planets { get; set; }
        public IList<IStar> Stars { get; set; }
        public Vector2 Coordinates { get; set; }
    }
}
