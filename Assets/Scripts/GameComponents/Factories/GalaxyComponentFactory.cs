using System.Collections.Generic;
using Assets.Scripts.API;
using UnityEngine;

namespace Assets.Scripts.GameComponents.Factories
{
	public static class GalaxyComponentFactory
	{
		public static IGalaxy Model { get; set; }
		public static IEnumerable<GalaxyComponent> Create()
		{
			var galaxy = new GalaxyComponent("Map");

			for (int i = 0; i < Model.Sectors.Count; i++)
			{
				int x = i % Model.Size;
				int y = i / Model.Size;
				var sectorModel = Model.Sectors[new Vector2(x, y)];
				var row = galaxy.CreateOrGetRow(y);

				SectorComponentFactory.Create(sectorModel, row, Model.Size);

				yield return galaxy;
			}
		}
	}
}
