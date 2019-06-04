using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.DTOs;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class OrbitProcessor : IOrbitProcessor
    {
        private Dictionary<IOrbit, int> _allOrbitsTrafficSpeed;
        public OrbitProcessor(List<OrbitCondition> orbits)
        {
            _allOrbitsTrafficSpeed = orbits.ToDictionary(key => key.Orbit, value => value.TrafficSpeed);
        }
        public int GetOrbitTrafficSpeed(IOrbit orbit)
        {
            int result = int.MaxValue;
            _allOrbitsTrafficSpeed.TryGetValue(orbit, out result);
            return result;
        }
    }
}
