using UnityEngine;

namespace Assets.Scripts.GameComponents.Input
{
    public class LightController : MonoBehaviour
    {
        public float Duration = 15f;

        private Vector3 _targetPosition;
        private Vector3 _rootPosition;
        private bool _doSomething;
        private float _startTime;

        private Vector3 _sunrise;
        private Vector3 _sunset;
        private Vector3 _center;


        void Awake()
        {
            _targetPosition = new Vector3();
            _rootPosition = new Vector3();
            _doSomething = false;
            _startTime = 0;

            _sunrise = new Vector3(-5, 0, 0);
            _sunset = new Vector3(5, 0, 0);
            _center = (_sunrise + _sunset) * 0.5f;
            _center -= new Vector3(0, 1f, 0);

        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetAxis("Horizontal") > 0)
            {
                _targetPosition = _sunset;
                _rootPosition = _sunrise;
                _doSomething = true;
                _startTime = Time.time;
            }
            if (UnityEngine.Input.GetAxis("Horizontal") < 0)
            {
                _targetPosition = _sunrise;
                _rootPosition = _sunset;
                _doSomething = true;
                _startTime = Time.time;
            }

            if (_targetPosition == transform.position)
                _doSomething = false;

            if (!_doSomething)
                return;

            float distanceDone = (Time.time - _startTime) / Duration;
            transform.position = Vector3.Slerp(_rootPosition - _center, _targetPosition - _center, distanceDone);
            transform.position += _center;

            transform.LookAt(_center);
        }
    }
}
