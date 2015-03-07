using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
	/// <summary>
	/// Also known as Map ;o)
	/// </summary>
	public class GalaxyComponent
	{
		public Dictionary<Vector2, SectorComponent> Sectors { get; set; }

		public GalaxyComponent()
		{
			Sectors = new Dictionary<Vector2, SectorComponent>();
		}
	}
}