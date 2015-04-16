using UnityEngine;
using System.Collections;
using Assets.Scripts.Extensions;

public class DrawLine : MonoBehaviour
{
	private LineRenderer _lineRenderer;
	private float _counter;
	private float _distance;

	public Transform Origin;
	public Transform Destination;

	public float LineDrawSpeed = 6f;

	// Use this for initialization
	void Start()
	{
		_lineRenderer = GetComponent<LineRenderer>();

		_lineRenderer.SetWidth(.2f, .2f);

		_distance = Vector3.Distance(Origin.position, Destination.position);

		_lineRenderer.SetPosition(0, Origin.position);

	}

	// Update is called once per frame
	void Update()
	{
		if (_counter < _distance * .1f)
		{
			_counter += .1f / LineDrawSpeed;
			float x = Mathf.Lerp(0, _distance, _counter);

			Vector3 pointA = Origin.position;
			Vector3 pointB = Destination.position;

			Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

			_lineRenderer.SetPosition(1, pointAlongLine);

			Debug.Log("{0}; {1}".FormatWith(_counter, _distance));
		}
	}
}
