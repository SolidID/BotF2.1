using UnityEngine;

namespace Assets.Scripts.GenericBehaviours
{
	public class RotateElement : MonoBehaviour
	{

		public float Speed = -10;

		void Update()
		{
			transform.Rotate(Vector3.up * Time.deltaTime * Speed);
		}
	}
}
