using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.Interface;

namespace Traffic.DTOs
{
    public class OptimalPathAndVehicle
    {
        public OptimalPathAndVehicle(IVehicle vehicle , List<OptimalRouteNode> routeNodes, int timeTaken)
        {
            Vehicle = vehicle;
            Route = routeNodes;
            TimeTaken = timeTaken;
        }

        public IVehicle Vehicle { get; }
        public List<OptimalRouteNode> Route { get; }
        public int TimeTaken { get; }
    }
}
