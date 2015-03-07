using UnityEngine;
using System.Collections;

public class RotatePlanet : MonoBehaviour {

	public float speed = -10;

	void Update () {
		transform.Rotate( Vector3.up * Time.deltaTime * speed);
	}
}
