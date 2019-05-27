using System.Collections.Generic;
using Traffic.Interface;

namespace Traffic.Interface
{
    public interface IOrbitProcessor
    {
        List<IOrbit> Orbits { get; }

        IOrbit GetOrbitFromName(string orbitName);
        List<IOrbit> GetOrbitsFromName(List<string> orbitNames);
    }
}