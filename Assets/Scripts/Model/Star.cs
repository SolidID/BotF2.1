using System.Collections.Generic;
using Assets.Scripts.API;

namespace Assets.Scripts.Model
{
    public class Star : IStar
    {
        public float Size { get; set; }
        public StarClass Class { get; set; }
        public IList<IResource> Resources { get; set; }
    }
}