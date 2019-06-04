using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.Interface;

namespace Traffic.DTOs
{
    public class OptimalRouteNode
    {
        public int TimeTakenInMinutes { get; set; }
        public ICity FromCity { get; }
        public ICity ToCity { get; }
        public List<IOrbit> Route { get; set; }

        public OptimalRouteNode(ICity fromCity, ICity toCity, int? timeTaken = null,  List<IOrbit> routes = null)
        {
            TimeTakenInMinutes = timeTaken ?? int.MaxValue;
            FromCity = fromCity ?? throw new ArgumentNullException(nameof(fromCity));
            ToCity = toCity ?? throw new ArgumentNullException(nameof(toCity));
            Route = routes ?? new List<IOrbit>();
        }

        public static OptimalRouteNode operator +(OptimalRouteNode first, OptimalRouteNode second)
        {
            return new OptimalRouteNode(first.FromCity, second.ToCity, first.TimeTakenInMinutes + second.TimeTakenInMinutes, first.Route.Concat(second.Route).ToList());
        }

    }
}
