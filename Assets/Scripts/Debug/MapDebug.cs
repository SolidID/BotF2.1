using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameComponents;
using UnityEngine;

namespace Assets.Scripts.Debug
{
    public class MapDebug : MonoBehaviour
    {
        void LateUpdate()
        {
            DebugOutput.Instance.AddMessage("Map: ");
            DebugOutput.Instance.AddMessage("Size: " + GetComponent<Renderer>().bounds.size);
            DebugOutput.Instance.AddMessage("min: " + GetComponent<Renderer>().bounds.min);
            DebugOutput.Instance.AddMessage("max: " + GetComponent<Renderer>().bounds.max);

        }
    }
}
