using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.API;
using Assets.Scripts.Configuration;
using Assets.Scripts.GameComponents.Input;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
    public static class SectorComponentFactory
    {
        public static SectorComponent Create(ISector model, GameObject row, int size)
        {
            var sectorHolder = new GameObject("SectorWrapper " + model.Coordinates.x);
            sectorHolder.AddComponent<SectorComponent>();
            var sector = sectorHolder.GetComponent<SectorComponent>();
            sector.Model = model;
            sector.transform.position = ToPixel(model.Coordinates, size);
            sector.InitializeComponents();

            sectorHolder.transform.parent = row.transform;
            return sector;
        }

        private static Vector3 ToPixel(Vector2 hexCoords, int size)
        {
            float correctedX = hexCoords.x + 1;
            float correctedY = hexCoords.y + 1;

            float x = (correctedX * Globals.Width) - (((int)correctedY & 1) * Globals.HalfWidth) - (GameSettings.Instance.GalaxySize * Globals.Width + Globals.HalfWidth) / 2f;
            float z = (correctedY * Globals.Radius + (correctedY - 1) * (Globals.Radius / 2)) - ((GameSettings.Instance.GalaxySize - 1) * 1.5f * Globals.Radius + Globals.Height) / 2f;

            return new Vector3(x, 0, z);
        }
    }
}
