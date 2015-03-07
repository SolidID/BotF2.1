using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.API
{
    public interface ISector
    {
        string Name { get; set; }
        IList<IPlanet> Planets { get; set; }
        IList<IStar> Stars { get; set; }
        Vector2 Coordinates { get; set; }
    }
}