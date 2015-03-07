using Assets.Scripts.API;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
	public static class GalaxyComponentFactory
	{
		public static IGalaxy Model { get; set; }
		public static IEnumerable<GalaxyComponent> Create()
		{
			//var galaxy = new GalaxyComponent();

			var map = new GameObject("Map"); // is the component at root level
			var rows = new Dictionary<int, GameObject>();

			for (int i = 0; i < Model.Sectors.Count; i++)
			{
				int x = i % Model.Size;
				int y = i / Model.Size;
				var sectorModel = Model.Sectors[new Vector2(x, y)];
				GameObject row;
				if (!rows.TryGetValue(y, out row))
				{
					row = new GameObject("Row " + y);
					rows.Add(y, row);
				}

				SectorComponentFactory.Create(sectorModel, row, Model.Size);

				row.transform.parent = map.transform;
				yield return null;
			}
		}
	}
}
