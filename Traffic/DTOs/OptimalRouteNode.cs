using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Interface;

namespace Traffic.DTOs
{
    public class OptimalRouteNode
    {
        public int TimeTakenInMinutes { get; set; }
        public List<IOrbit> Route { get; set; }

        public OptimalRouteNode(int? timeTaken = null, List<IOrbit> routes = null)
        {
            TimeTakenInMinutes = timeTaken ?? int.MaxValue;
            Route = routes ?? new List<IOrbit>();
        }
    }
}
