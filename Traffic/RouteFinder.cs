using System;
using System.Collections.Generic;
using Traffic.DTOs;
using Traffic.Enum;
using Traffic.Interface;

namespace Traffic
{
    public class RouteFinder
    {

        public RouteFinder(ICitiesGraph citiesGraph, IOrbitProcessor orbitProcessor, IVehiclesProcessor vehiclesProcessor)
        {
            CitiesGraph = citiesGraph;
            OrbitProcessor = orbitProcessor;
            VehiclesProcessor = vehiclesProcessor;
        }

        public ICitiesGraph CitiesGraph { get; }
        public IOrbitProcessor OrbitProcessor { get; }
        public IVehiclesProcessor VehiclesProcessor { get; }

        public OptimalPathAndVehicle OptimalRoute(List<OrbitCondition> orbits, WeatherConditions weatherCondition)
        {
            var possibleVehicles = VehiclesProcessor.Vehicles;
            int minTimeSoFar = int.MaxValue;
            OptimalPathAndVehicle result = null;
            //foreach (var orbit in orbits)
            //{
            //    IOrbit currentOrbit = OrbitProcessor.GetOrbitFromName(orbit.OrbitName);
            //    foreach (var vehicle in possibleVehicles)
            //    {
            //        if(vehicle.TravellableConditons.HasFlag(weatherCondition))
            //        {
            //            int currentTimeTaken = ComputeTimeTaken(currentOrbit, vehicle, weatherCondition, orbit.TrafficSpeed);
            //            if(currentTimeTaken < minTimeSoFar)
            //            {
            //                result = new OptimalPathAndVehicle(vehicle, currentOrbit);
            //                minTimeSoFar = currentTimeTaken;
            //            }
            //        }
            //    }
            //}
            if (result == null)
                throw new Exception("No optimal Route found");
            return result;
        }

        public OptimalPathAndVehicle OptimalRoute(List<OrbitCondition> orbits, WeatherConditions weatherCondition)
        {

        }

        public List<List<Dictionary<IVehicle, OptimalRouteNode>>> AllPairShortestPath()
        {
            var shortestPath = new List<List<Dictionary<IVehicle, OptimalRouteNode>>>();

            //Initialize Empty initial shortest path
            for (int i = 0; i < CitiesGraph.TotalCities; i++)
            {
                shortestPath.Add(new List<Dictionary<IVehicle, OptimalRouteNode>>());
                for (int j = 0; j < CitiesGraph.TotalCities; j++)
                {
                    var allVehicles = new Dictionary<IVehicle, OptimalRouteNode>();
                    foreach (var vehicle in VehiclesProcessor.Vehicles)
                    {
                        allVehicles.Add(vehicle, new OptimalRouteNode());
                    }
                    shortestPath[i].Add(allVehicles);
                }
            }

            //Initialize existing orbits to shortest path
            foreach (var city in CitiesGraph.CitiesMap)
            {
                foreach (var edge in city.Value)
                {

                }
            }

            return shortestPath;
        }
        private int ComputeTimeTaken(IOrbit orbit, IVehicle vehicle, WeatherConditions weatherCondition, int trafficSpeed)
        {
            int craterReductionPercentage = WeatherFactory.GetCraterReductionPercentage(weatherCondition);
            int reducedCrater = Convert.ToInt32((craterReductionPercentage /100m) * orbit.Craters);

            int totalCraters = orbit.Craters + reducedCrater;
            int vehicleSpeed = Math.Min(vehicle.SpeedInMMPerHr, trafficSpeed);

            int timeTakeninMinutes = vehicle.TimeToCrossCraterInMinutes * totalCraters;
            timeTakeninMinutes += Convert.ToInt16((orbit.DistanceInMM / vehicleSpeed) * 60);
            return timeTakeninMinutes;
        }
    }
}
