using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Model
{
	public static class SectorNameManager
	{
		private static List<string> _names;

		static SectorNameManager()
		{
			var textAsset = Resources.Load("Settings/system_names") as TextAsset;
			if (textAsset == null)
				return;

			_names = new List<string>();

			foreach (string name in textAsset.text.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
			{
				_names.Add(name.Trim());
			}
		}

		public static string GetName()
		{
			return _names[Random.Range(0, _names.Count - 1)];
		}
	}
}
