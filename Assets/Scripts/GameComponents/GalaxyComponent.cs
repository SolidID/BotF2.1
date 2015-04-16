using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
	/// <summary>
	/// Also known as Map ;o)
	/// </summary>
	public class GalaxyComponent
	{
		private readonly GameObject _map;
		private Dictionary<int, GameObject> _rows;

		public GalaxyComponent(String name)
		{
			_map = new GameObject(name);
			_rows = new Dictionary<int, GameObject>();
		}

		public void AddChildToHierarchy(GameObject child)
		{
			child.transform.parent = _map.transform;
		}

		public GameObject CreateOrGetRow(int index)
		{
			GameObject row;
			if (_rows.TryGetValue(index, out row))
				return row;

			row = new GameObject("Row " + index);
			_rows.Add(index, row);
			AddChildToHierarchy(row);

			return row;
		}
	}
}