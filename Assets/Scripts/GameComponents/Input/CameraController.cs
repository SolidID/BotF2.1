using Assets.Scripts.Configuration;
using Assets.Scripts.Debug;
using Assets.Scripts.Extensions;
using BotF2.Core.Extensions;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.GameComponents.Input
{
	public class CameraController : MonoBehaviour
	{
		private static CameraController _instance;

		public static CameraController Instance
		{
			get { return _instance ?? (_instance = FindObjectOfType<CameraController>()); }
		}

		private float _maxZoomOutLevel = -1;
		private float _maxZoomInLevel = 10f;
		public float ScrollSpeed = 0.2f;
		public float ZoomSpeed = 75;


		private bool _isInDragMode;
		private Vector3 _dragStartCoords;
		private Vector3 _cameraDragStartCoords;
		private float _targetOrtographicSize;
		private Vector3 _targetPostition;

		private static Transform CameraHolder
		{
			get { return Camera.main.transform.parent; }
		}

		private static bool IsOrthoGraphic
		{
			get { return Camera.main.orthographic; }
		}

		private static float MapWidth
		{
			get { return (GameSettings.Instance.GalaxySize * Globals.Width + Globals.HalfWidth); }
		}

		private static float MapHeight
		{
			get { return ((GameSettings.Instance.GalaxySize - 1) * 1.5f * Globals.Radius + Globals.Height); }
		}

		public float Distance { get; private set; }
		public Plane[] Planes { get; private set; }

		void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
			}
			else
			{
				if (_instance != this)
					Destroy(gameObject);
			}
		}

		void Update()
		{
			if (IsOrthoGraphic)
			{
				Distance = Camera.main.orthographicSize;
			}
			else
			{
				Distance = Camera.main.transform.position.y;
			}
			Planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		}

		void LateUpdate()
		{

			DebugOutput.Instance.AddMessage("IsPointerOverUI: {0}".FormatWith(UISystem.Instance.IsMouseOverUiElement()));
			// do not scroll, zoom etc when the mouse is over a UI Element
			//if (UISystem.Instance.IsMouseOverUiElement())
			//	return;

			if (IsOrthoGraphic)
			{
				if (Math.Abs(_maxZoomOutLevel - (-1f)) < 0.0001f) // fix the initial value
				{
					_maxZoomOutLevel = Mathf.Min((MapWidth * (1 / (Screen.width / (float)Screen.height))) / 2f, MapHeight / 2f);
					_targetOrtographicSize = _maxZoomOutLevel / 2;
				}

				HandleOrthoGraphicZoom();
				HandleOrthographicMovement();
				HandleCameraRotation();

				CorrectCameraPosition();
			}
			else
			{
				HandlePerspectiveZoom();
				HandlePerspectiveMovement();
				HandleCameraRotation();

				//CorrectCameraPosition();
			}
		}

		private static void HandleCameraRotation()
		{
			if (!UnityEngine.Input.GetMouseButton(1)) return;

			Vector3 currentRotation = CameraHolder.eulerAngles;

			float desiredX = currentRotation.x + (-1 * UnityEngine.Input.GetAxis("Mouse Y") * GameSettings.Instance.VerticalMouseSensitivity);
			float desiredY = currentRotation.y + UnityEngine.Input.GetAxis("Mouse X") * GameSettings.Instance.HorizontalMouseSensitivity;

			if (Math.Abs(GameSettings.Instance.VerticalMouseSensitivity) > 0.0000f)
				currentRotation.x = desiredX.Clamp(10, 87);

			if (Math.Abs(GameSettings.Instance.HorizontalMouseSensitivity) > 0.0000f)
				currentRotation.y = desiredY;

			CameraHolder.eulerAngles = currentRotation;
		}

		private void HandlePerspectiveMovement()
		{
			HandlePerspectiveCameraMovementByMouseDrag();
		}

		private void HandlePerspectiveCameraMovementByMouseDrag()
		{
			//if (UnityEngine.Input.GetMouseButtonDown(0))
			//{
			//	if (!_isInDragMode)
			//	{
			//		RaycastHit hit;
			//		Physics.Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out hit);

			//		_dragStartCoords = GameObject.Find("Map").transform.position - hit.point;
			//	}
			//	_isInDragMode = true;
			//}

			//if (UnityEngine.Input.GetMouseButtonUp(0))
			//{
			//	_isInDragMode = false;
			//}


			//if (_isInDragMode)
			//{
			//	//Get the screen coordinate of some point
			//	RaycastHit hit;
			//	Physics.Raycast(Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition), out hit);
			//	if (hit.transform != null)
			//	{
			//		GameObject.Find("Map").transform.position = _dragStartCoords + hit.point;
			//		DebugOutput.Instance.AddMessage("{0}|{1}|{2}".FormatWith(GameObject.Find("Map").transform.position, _dragStartCoords, hit.point));
			//	}
			//}

			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				_isInDragMode = true;
				_dragStartCoordinates = UnityEngine.Input.mousePosition;
				_cameraDragStartPosition = Camera.main.transform.position;
			}

			if (UnityEngine.Input.GetMouseButtonUp(0))
			{
				_isInDragMode = false;
				_dragStartCoordinates = default(Vector3);
			}

			// allways assuming that point 0 for the y is my game "plane"
			// I the height of the camera 
			float myCurrentHeight = Camera.main.transform.position.y;

			// I know the angle of the Field of view
			float angle = Camera.main.fieldOfView;

			// I know the ratio of the screen
			float ratio = (float)Screen.width / Screen.height;

			// so I can calculate how many units I see at level 0
			float c = myCurrentHeight;
			float alpha = angle / 2f;
			float beta = 90f;
			float gamma = 180f - alpha - beta;
			//b / c = sin(beta) / sin(gamma) => b = sin(beta) / sin(gamma) * c
			float b = Mathf.Sin(beta * Mathf.Deg2Rad) / (float)Math.Sin(gamma * Mathf.Deg2Rad) * c;

			// then my a is 
			var a = (float)Math.Sqrt(b * b - c * c);

			// a is the units i see half the way up. so...
			float verticalUnits = 2 * a;
			float horizontalUnits = verticalUnits * ratio;


			if (IsDragging)
			{
				// calculate camera movement (everything is relative)
				Vector3 moved = _dragStartCoordinates - UnityEngine.Input.mousePosition;
				Camera.main.transform.position = _cameraDragStartPosition + new Vector3(horizontalUnits * moved.x / Screen.width, 0, verticalUnits * moved.y / Screen.height);
			}

		}

		public bool IsDragging { get { return _isInDragMode && _dragStartCoordinates != UnityEngine.Input.mousePosition; } }

		private void HandleOrthographicMovement()
		{
			if (HandleCameraMovementByMouseDrag()) return;

			HandleCameraMovemtByKeyBoard();
		}

		private bool HandleCameraMovementByMouseDrag()
		{
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				if (!_isInDragMode)
				{
					_dragStartCoords = new Vector3(UnityEngine.Input.mousePosition.x, 0, UnityEngine.Input.mousePosition.y);
					_cameraDragStartCoords = CameraHolder.position;
				}
				_isInDragMode = true;
			}

			if (UnityEngine.Input.GetMouseButtonUp(0))
			{
				_isInDragMode = false;
			}


			if (_isInDragMode)
			{
				// How do I calculate the position?
				// well I have to clac what is a Pixel on the Screen in Unity units.
				// First think of it like rows and cols you see on the screen.
				// On Size 1 for the ortho cam you always have 2 rows. 
				// But how many Cols you see depends on the ratio of the game window. The only real constant here is that you have 2 rows on size 1.
				// That means that you can calculate the factor for the cols as follows: x= Screen.Width/Screen.Height * 2 <-- Screen.with/Screen.Height is the ratio; 2 is the const of 2 rows
				// So if the orto size of the cam increases you have to multiply the factor by the size also.
				// the result would be this:
				//  float ratio = Screen.width / (float)Screen.height;
				//  float size = Camera.main.orthographicSize;
				//  var y = mouseMovement.y / (Screen.height / (2 * size));
				//  var x = mouseMovement.x / (Screen.width / (2 * ratio * size));

				float ratio = Screen.width / (float)Screen.height;
				float size = Camera.main.orthographicSize;
				Vector3 mouseMovement = (new Vector3(UnityEngine.Input.mousePosition.x, 0, UnityEngine.Input.mousePosition.y) - _dragStartCoords);

				var y = mouseMovement.z / (Screen.height / (2 * size));
				var x = mouseMovement.x / (Screen.width / (2 * ratio * size));

				Vector3 calculatedCameraMovement = Vector3.Scale(Camera.main.transform.TransformDirection(new Vector3(x, y, 0)), new Vector3(1, 0, 1));
				Vector3 position = _cameraDragStartCoords - calculatedCameraMovement;

				CameraHolder.position = _targetPostition = position;

				return true;
			}
			return false;
		}

		private void CorrectCameraPosition()
		{
			Vector3 position = CameraHolder.position;
			float ratio = Screen.width / (float)Screen.height;

			float leftEdge = MapWidth / -2f;
			float rightEdge = -1 * leftEdge;
			float upperEdge = MapHeight / 2f;
			float lowerEdge = -1 * upperEdge;

			float size = Camera.main.orthographicSize = Camera.main.orthographicSize.Clamp(_maxZoomInLevel, _maxZoomOutLevel);

			CameraHolder.position = new Vector3
			(
				position.x.Clamp(leftEdge + (2f * ratio * size) / 4, rightEdge - (2f * ratio * size) / 4),
				0,
				position.z.Clamp(lowerEdge + (2f * size) / 4, upperEdge - (2f * size) / 4)
			);

			_targetPostition = new Vector3
			(
				_targetPostition.x.Clamp(leftEdge + (2f * ratio * size) / 4, rightEdge - (2f * ratio * size) / 4),
				0,
				_targetPostition.z.Clamp(lowerEdge + (2f * size) / 4, upperEdge - (2f * size) / 4)
			);

		}

		private void HandleCameraMovemtByKeyBoard()
		{
			float horizontal = UnityEngine.Input.GetAxis("Horizontal");
			float vertical = UnityEngine.Input.GetAxis("Vertical");
			float size = Camera.main.orthographicSize;

			if (horizontal < 0)
			{
				_targetPostition -= CameraHolder.transform.right * size * Time.deltaTime * ScrollSpeed;
			}
			else if (horizontal > 0)
			{
				_targetPostition += CameraHolder.transform.right * size * Time.deltaTime * ScrollSpeed;
			}
			if (vertical < 0)
			{
				_targetPostition -= CameraHolder.transform.forward * size * Time.deltaTime * ScrollSpeed;
			}
			else if (vertical > 0)
			{
				_targetPostition += CameraHolder.transform.forward * size * Time.deltaTime * ScrollSpeed;
			}

			CameraHolder.position = Vector3.Scale(_targetPostition, new Vector3(1, 0, 1));
		}

		private float _perspectiveTargetY = 100f;
		private Vector3 _dragStartCoordinates;
		private Vector3 _cameraDragStartPosition;

		private void HandlePerspectiveZoom()
		{
			float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
			if (scroll < 0)
			{
				_perspectiveTargetY = Mathf.Min(_perspectiveTargetY * 1.2f, 1000);
			}
			else if (scroll > 0)
			{
				_perspectiveTargetY = Mathf.Max(_perspectiveTargetY / 1.2f, 1);
			}

			var currentY = CameraHolder.position.y;
			var distance = Mathf.Abs(_perspectiveTargetY - currentY);
			DebugOutput.Instance.AddMessage("Disctance: " + distance);
			CameraHolder.position = new Vector3(Camera.main.transform.position.x, distance > 0.33f
				? Mathf.Lerp(currentY, _perspectiveTargetY, ZoomSpeed * Time.deltaTime)
				: _perspectiveTargetY, Camera.main.transform.position.z);
		}

		private void HandleOrthoGraphicZoom()
		{
			float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
			if (scroll < 0) // Zoom in
			{
				_targetOrtographicSize = Mathf.Min(_targetOrtographicSize * 1.2f, _maxZoomOutLevel);
			}
			else if (scroll > 0) // zoom out
			{
				_targetOrtographicSize = Mathf.Max(_targetOrtographicSize / 1.2f, _maxZoomInLevel);
			}

			var currentSize = Camera.main.orthographicSize;
			var distance = Mathf.Abs(_targetOrtographicSize - currentSize);
			DebugOutput.Instance.AddMessage("Disctance: " + distance);
			Camera.main.orthographicSize = distance > 0.33f
				? Mathf.Lerp(currentSize, _targetOrtographicSize, ZoomSpeed * Time.deltaTime)
				: _targetOrtographicSize;

		}
	}
}
