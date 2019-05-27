using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Interface
{
    public interface IEdge
    {
        IOrbit Orbit { get; }
        ICity ToCity { get; }
    }
}
