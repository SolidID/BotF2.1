using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Debug;
using Assets.Scripts.Extensions;
using JetBrains.Annotations;

public class UISystem : MonoBehaviour
{
	private int _counter = 0;
	private List<RectTransform> _rects;
	private static UISystem _instance;

	public static UISystem Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<UISystem>();
			}
			return _instance;
		}
	}

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

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (_counter++ == 100)
			_rects = FindObjectsOfType<RectTransform>().Where(r => r.gameObject.activeInHierarchy).ToList();

		if (_rects != null)
			DebugOutput.Instance.AddMessage("Rects: {0}".FormatWith(_rects.Count));

	}

	public bool IsMouseOverUiElement()
	{
		if (_rects == null || _rects.Count == 0)
			return false;

		return _rects.Where(InVectorInRect).Any();
	}

	private static bool InVectorInRect(RectTransform rect)
	{
		return rect.rect.Contains(Input.mousePosition);
	}
}
