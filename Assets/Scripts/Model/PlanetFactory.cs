using System.Collections.Generic;
using Assets.Scripts.API;
using Assets.Scripts.Configuration;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public static class PlanetFactory
    {
        public static IList<IPlanet> CreateRandom(string sectorName, int planetCount)
        {
            List<IPlanet> returnList = new List<IPlanet>(planetCount);
            for (int i = 0; i < planetCount; i++)
            {
                returnList.Add(new Planet()
                {
                    Class = PlanetClass.M,
                    Name = sectorName + " " + (i + 1).ToRoman(),
                    Resources = ResourceFactory.CreateRandom()
                });
            }
            return returnList;
        }
    }
}