using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Interface;

namespace Traffic.DTOs
{
    public class OrbitCondition
    {
        public OrbitCondition(IOrbit orbit, int trafficSpeed)
        {
            Orbit = orbit;
            TrafficSpeed = trafficSpeed;
        }

        public IOrbit Orbit { get; }
        public int TrafficSpeed { get; }
    }
}
