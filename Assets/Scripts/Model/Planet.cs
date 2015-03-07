using System.Collections.Generic;
using Assets.Scripts.API;

namespace Assets.Scripts.Model
{
    public class Planet : IPlanet
    {
        public string Name { get; set; }
        public PlanetClass Class { get; set; }
        public IList<IResource> Resources { get; set; }
    }
}