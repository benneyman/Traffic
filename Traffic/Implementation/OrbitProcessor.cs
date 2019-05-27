using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class OrbitProcessor : IOrbitProcessor
    {
        private Dictionary<string, IOrbit> _allOrbits;
        public List<IOrbit> Orbits  { get; }

        public OrbitProcessor(List<IOrbit> orbits)
        {
            _allOrbits = orbits.ToDictionary(key => key.Name, value => value);
            Orbits = orbits;
        }

        public IOrbit GetOrbitFromName(string orbitName)
        {
            IOrbit orbit;
            if (!_allOrbits.TryGetValue(orbitName, out orbit))
                throw new KeyNotFoundException($"{orbit.Name} not found");
            return orbit;
        }
        public List<IOrbit> GetOrbitsFromName(List<string> orbitNames)
        {
            try
            {
                List<IOrbit> orbits = new List<IOrbit>();
                foreach (var orbitName in orbitNames)
                {
                    orbits.Add(GetOrbitFromName(orbitName));
                }
                return orbits;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }
    }
}
