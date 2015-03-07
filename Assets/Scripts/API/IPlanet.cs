using System.Collections.Generic;

namespace Assets.Scripts.API
{
    public interface IPlanet
    {
        string Name { get; }
        PlanetClass Class { get; }
        IList<IResource> Resources { get; }
    }
}