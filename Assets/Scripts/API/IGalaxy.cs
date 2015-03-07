using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.API
{
    public interface IGalaxy
    {
        Dictionary<Vector2, ISector> Sectors { get; set; }
        int Size { get; set; }
    }
}