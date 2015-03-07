using System.Collections.Generic;

namespace Assets.Scripts.API
{
    public interface IStar
    {
        float Size { get; }
        StarClass Class { get; }
        IList<IResource> Resources { get; }
    }
}