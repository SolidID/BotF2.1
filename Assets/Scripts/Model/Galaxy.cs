using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.API;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Galaxy : IGalaxy
    {
        public Dictionary<Vector2, ISector> Sectors { get; set; }
        public int Size { get; set; }
    }
}
