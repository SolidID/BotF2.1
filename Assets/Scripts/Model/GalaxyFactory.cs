using System.Collections.Generic;
using Assets.Scripts.API;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public static class GalaxyFactory
    {
        public static IGalaxy Create(int size)
        {
            var galaxy = new Galaxy()
            {
                Size = size
            };

            var sectors = new Dictionary<Vector2, ISector>(size * size);
            if (size > 1)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        sectors.Add(new Vector2(x, y), SectorFactory.Create(x, y));
                    }
                }
            }
            else
            {
                sectors.Add(new Vector2(1, 1), SectorFactory.Create(1, 1));
            }

            galaxy.Sectors = sectors;

            return galaxy;
        }

        public static IGalaxy Create(GalaxySize size)
        {
            return Create(size.ToInt32());
        }
    }
}
