using System;
using UnityEngine;

namespace Assets
{
    public class DisplacementSlider : MonoBehaviour
    {
        private float _value = 0f;

        public Vector2 Position;
        public Vector2 Size;
        public Vector2 MinMax;
        public string ValueName;
        public GameObject Planet;

        // Use this for initialization
        void Start()
        {
            _value = Planet.GetComponent<Renderer>().material.GetFloat(ValueName);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void LateUpdate()
        {
            GetComponent<GUIText>().text = ValueName + " value: " + _value;
        }

        void OnGUI()
        {
            float newValue = GUI.HorizontalSlider(new Rect(Position.x, Position.y, Position.x + Size.x, Size.y + Position.y), _value, MinMax.x, MinMax.y);
            if (Math.Abs(newValue - _value) > 0.0001)
            {
                Planet.GetComponent<Renderer>().material.SetFloat(ValueName, newValue);
                _value = newValue;
            }
        }
    }
}
