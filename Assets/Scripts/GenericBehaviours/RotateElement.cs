using Assets.Scripts.GameComponents.Input;
using UnityEngine;

namespace Assets.Scripts.GenericBehaviours
{
	public class RotateElement : MonoBehaviour
	{

		public float Speed = -10;
		private bool _shouldRotate = true;
		private int _counter;

		void Update()
		{
			UpdateShouldRotate();
			// rotate the object only when it is visible
			if (_shouldRotate && CameraController.Instance.Distance < 150f)
				transform.Rotate(Vector3.up * Time.deltaTime * Speed);
		}

		private void UpdateShouldRotate()
		{
			if (_counter == 10)
			{
				var myCollider = GetComponent<Collider>();
				if (myCollider != null)
				{
					_shouldRotate = GeometryUtility.TestPlanesAABB(CameraController.Instance.Planes, myCollider.bounds);
				}
				_counter = 0;
				return;
			}

			_counter++;
		}
	}
}
