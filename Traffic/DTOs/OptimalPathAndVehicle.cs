using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Interface;

namespace Traffic.DTOs
{
    public class OptimalPathAndVehicle
    {
        public OptimalPathAndVehicle(IVehicle vehicle , IOrbit orbit)
        {
            Vehicle = vehicle;
            Orbit = orbit;
        }

        public IVehicle Vehicle { get; }
        public IOrbit Orbit { get; }
    }
}
