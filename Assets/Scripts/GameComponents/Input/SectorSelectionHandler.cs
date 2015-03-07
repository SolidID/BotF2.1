using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameComponents.Input
{
	public static class SectorSelectionHandler
	{
		private static SectorComponent _selectedSectorComponent;
		public static SectorComponent SelectedSectorComponent
		{
			get { return _selectedSectorComponent; }
			set
			{
				if (_selectedSectorComponent != null && value != _selectedSectorComponent)
				{
					_selectedSectorComponent.Deselect();
				}
				_selectedSectorComponent = value;
			}
		}
	}
}
