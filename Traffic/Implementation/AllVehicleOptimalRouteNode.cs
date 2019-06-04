using System;
using System.Collections.Generic;
using System.Text;
using Traffic.DTOs;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class AllVehicleOptimalRouteNode
    {
        public  Dictionary<IVehicle, OptimalRouteNode> VehicleMinCost { get; set; }
        public List<IVehicle> Vehicles { get;  }

        public AllVehicleOptimalRouteNode(ICity fromCity, ICity toCity, List<IVehicle> vehicles, int? timeTaken = null)
        {
            Vehicles = vehicles;
            VehicleMinCost = new Dictionary<IVehicle, OptimalRouteNode>();
            foreach (var vehicle in vehicles)
            {
                VehicleMinCost.Add(vehicle, new OptimalRouteNode(fromCity, toCity, timeTaken));
            }
        }
        public AllVehicleOptimalRouteNode(Dictionary<IVehicle, OptimalRouteNode> vehicleMinCost)
        {
            VehicleMinCost = vehicleMinCost ?? throw new ArgumentNullException(nameof(vehicleMinCost));
            Vehicles = new List<IVehicle>(vehicleMinCost.Keys);
        }
        public OptimalRouteNode GetOptimalRouteNode(IVehicle vehicle)
        {
            OptimalRouteNode routeNode;
            if (!VehicleMinCost.TryGetValue(vehicle, out routeNode))
                throw new KeyNotFoundException($"{vehicle.VehicleType.ToString()} not present.");
            return routeNode;
        }
        public IVehicle GetMinimumCostVehicle()
        {
            IVehicle vehicle = null; int minTimeTaken = int.MaxValue;
            foreach (var item in VehicleMinCost.Keys)
            {
                if(VehicleMinCost[item].TimeTakenInMinutes < minTimeTaken)
                {
                    vehicle = item;
                    minTimeTaken = VehicleMinCost[item].TimeTakenInMinutes;
                }
            }
            return vehicle;
        }
        public OptimalRouteNode GetMinimumCostRouteNode()
        {
            IVehicle vehicle = null; int minTimeTaken = int.MaxValue;
            foreach (var item in VehicleMinCost.Keys)
            {
                if (VehicleMinCost[item].TimeTakenInMinutes < minTimeTaken)
                {
                    vehicle = item;
                    minTimeTaken = VehicleMinCost[item].TimeTakenInMinutes;
                }
            }
            return VehicleMinCost[vehicle];
        }
        public int GetMinimumTimeTaken()
        {
            int minTimeTaken = int.MaxValue;
            foreach (var item in VehicleMinCost.Keys)
            {
                if (VehicleMinCost[item].TimeTakenInMinutes < minTimeTaken)
                {
                    minTimeTaken = VehicleMinCost[item].TimeTakenInMinutes;
                }
            }
            return minTimeTaken;
        }   
        public static AllVehicleOptimalRouteNode operator +(AllVehicleOptimalRouteNode first, AllVehicleOptimalRouteNode second)
        {
            Dictionary<IVehicle, OptimalRouteNode> minCost = new Dictionary<IVehicle, OptimalRouteNode>();
            foreach (var vehicle in first.Vehicles)
            {
                OptimalRouteNode firstOptimal = first.GetOptimalRouteNode(vehicle);
                OptimalRouteNode secondOptimal = second.GetOptimalRouteNode(vehicle);
                OptimalRouteNode optimalRoute = firstOptimal + secondOptimal;
                minCost.Add(vehicle, optimalRoute);
            }
            return new AllVehicleOptimalRouteNode(minCost);
        }
    }
}
