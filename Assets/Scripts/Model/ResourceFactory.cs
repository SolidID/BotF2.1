using System.Collections.Generic;
using Assets.Scripts.API;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public static class ResourceFactory
    {
        private static readonly List<string> _resourceNames = new List<string>
        {
            "Energy",
            "Dilithium",
            "Food",
            "Production",
        };

        public static IList<IResource> CreateRandom()
        {
            IList<IResource> returnList = new List<IResource>();
            if (Random.Range(0, 10) % 5 == 0)
            {
                int rand = Random.Range(1, 3);
                for (int i = 0; i < rand; i++)
                {
                    returnList.Add(new Resource() { Name = _resourceNames[Random.Range(0, _resourceNames.Count - 1)] });
                }
            }

            return returnList;
        }
    }
}