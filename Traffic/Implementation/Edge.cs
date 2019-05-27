using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class Edge : IEdge
    {
        public Edge(ICity toCity, IOrbit orbit)
        {
            ToCity = toCity;
            Orbit = orbit;
        }

        public ICity ToCity { get; }
        public IOrbit Orbit { get; }
    }
}
