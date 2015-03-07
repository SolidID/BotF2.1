using Assets.Scripts.API;
using Assets.Scripts.Configuration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Model
{
    public static class SectorFactory
    {
        public static ISector Create(int x, int y)
        {
            string sectorName = "Sector " + (x + 1) + ":" + (y + 1);
            int rand = 0;

            if ((Random.Range(0, GameSettings.Instance.GalaxySize) % 10 <= 2))
            {
                rand = (int)(Random.value * 10);
                sectorName = SectorNameManager.GetName();
            }

            return new Sector()
            {
                Coordinates = new Vector2(x, y),
                Planets = PlanetFactory.CreateRandom(sectorName, rand),
                Name = sectorName
            };
        }
    }
}
