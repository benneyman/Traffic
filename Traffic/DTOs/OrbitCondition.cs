using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.DTOs
{
    public class OrbitCondition
    {
        public OrbitCondition(string orbitName, int trafficSpeed)
        {
            OrbitName = orbitName;
            TrafficSpeed = trafficSpeed;
        }

        public string OrbitName { get; }
        public int TrafficSpeed { get; }
    }
}
