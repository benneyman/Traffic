using System;
using System.Collections.Generic;
using System.Linq;
using Traffic.DTOs;
using Traffic.Enum;
using Traffic.Extentions;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class RouteFinder : IRouteFinder
    {
        public RouteFinder(ICitiesGraph citiesGraph, IVehiclesProcessor vehiclesProcessor, IWeatherFactory weatherFactory)
        {
            CitiesGraph = citiesGraph;
            VehiclesProcessor = vehiclesProcessor;
            WeatherFactory = weatherFactory;
        }

        public ICitiesGraph CitiesGraph { get; }
        public IVehiclesProcessor VehiclesProcessor { get; }
        public IWeatherFactory WeatherFactory { get; }

        public OptimalPathAndVehicle OptimalRoute(IOrbitProcessor orbitProcessor, WeatherConditions weatherCondition, ICity startCity, List<ICity> stops)
        {
            List<ICity> allStops = stops ?? new List<ICity>();
            var shortestPath = AllPairShortestPath(weatherCondition, orbitProcessor);
            return FindShortestRouteWithIntermididateCities(shortestPath, startCity, allStops);
        }

        private OptimalPathAndVehicle FindShortestRouteWithIntermididateCities(List<List<AllVehicleOptimalRouteNode>> shortestPath, ICity startCity, List<ICity> stops)
        {
            int totalPossibilities = stops.Count * (stops.Count + 1) / 2;
            List<AllVehicleOptimalRouteNode> overallOptimalRoute = new List<AllVehicleOptimalRouteNode>();
            AllVehicleOptimalRouteNode overallRouteMinimumCost = null;
            while (totalPossibilities > 0)
            {
                List<AllVehicleOptimalRouteNode> currentOptimalRoute = new List<AllVehicleOptimalRouteNode>();
                ICity from = startCity, to;
                AllVehicleOptimalRouteNode currentRouteMinimumCost = null;
                for (int i = 0; i < stops.Count; ++i)
                {
                    to = stops[i];
                    AllVehicleOptimalRouteNode currentOptimalRouteForCityPair = shortestPath[from.CityId - 1][to.CityId - 1];
                    currentOptimalRoute.Add(currentOptimalRouteForCityPair);
                    if(currentRouteMinimumCost == null)
                    {
                        currentRouteMinimumCost = currentOptimalRouteForCityPair;
                    }
                    else
                    {
                        currentRouteMinimumCost += currentOptimalRouteForCityPair;
                    }
                    from = to;
                }
                if(overallRouteMinimumCost == null || overallRouteMinimumCost.GetMinimumTimeTaken() > currentRouteMinimumCost.GetMinimumTimeTaken())
                {
                    overallRouteMinimumCost = currentRouteMinimumCost;
                    overallOptimalRoute = currentOptimalRoute;
                }
                stops.NextPermutation();
                --totalPossibilities;
            }
            IVehicle minCostVehicle = overallRouteMinimumCost.GetMinimumCostVehicle();
            List<OptimalRouteNode> optimalRouteNodes = overallOptimalRoute.Select(m => m.GetOptimalRouteNode(minCostVehicle))
                .ToList();
            int minTimeTaken = optimalRouteNodes.Select(m => m.TimeTakenInMinutes).Sum();
            return new OptimalPathAndVehicle(minCostVehicle, optimalRouteNodes, minTimeTaken);
        }

    public List<List<AllVehicleOptimalRouteNode>> AllPairShortestPath(WeatherConditions weatherCondition, IOrbitProcessor orbitProcessor)
    {
        var shortestPath = new List<List<AllVehicleOptimalRouteNode>>();

        //Initialize Empty initial shortest path
        foreach (var fromCity in CitiesGraph.CitiesMap)
        {
            shortestPath.Add(new List<AllVehicleOptimalRouteNode>());
            foreach (var toCity in CitiesGraph.CitiesMap)
            {
                int timeTaken = fromCity.Key.Equals(toCity.Key) ? 0 : int.MaxValue;
                var allVehiclesShortestPath = new AllVehicleOptimalRouteNode(fromCity.Key, toCity.Key, VehiclesProcessor.GetSuitableVehiclesForWeather(weatherCondition), timeTaken);
                shortestPath[fromCity.Key.CityId - 1].Add(allVehiclesShortestPath);
            }
        }
        //Initialize existing orbits to shortest path
        foreach (var city in CitiesGraph.CitiesMap)
        {
            foreach (var edge in city.Value)
            {
                foreach (var vehicle in VehiclesProcessor.GetSuitableVehiclesForWeather(weatherCondition))
                {
                    AllVehicleOptimalRouteNode currentShortestPathForAllVehicles = shortestPath[city.Key.CityId - 1][edge.ToCity.CityId - 1];
                    OptimalRouteNode optimalRouteNodeForCurrentVehicle = currentShortestPathForAllVehicles.GetOptimalRouteNode(vehicle);
                    int trafficSpeed = Math.Min(vehicle.SpeedInMMPerHr, orbitProcessor.GetOrbitTrafficSpeed(edge.Orbit));
                    int timeTaken = ComputeTimeTaken(edge.Orbit, vehicle, weatherCondition, trafficSpeed);
                    optimalRouteNodeForCurrentVehicle.TimeTakenInMinutes = timeTaken;
                    optimalRouteNodeForCurrentVehicle.Route = new List<IOrbit>() { edge.Orbit };
                }
            }
        }

        for (int k = 0; k < CitiesGraph.TotalCities; k++)
        {
            for (int i = 0; i < CitiesGraph.TotalCities; i++)
            {
                for (int j = 0; j < CitiesGraph.TotalCities; j++)
                {
                    AllVehicleOptimalRouteNode allVehicleOptimalRouteNode = shortestPath[i][j];
                    foreach (var vehicle in allVehicleOptimalRouteNode.Vehicles)
                    {
                        OptimalRouteNode vehicleOptimal = allVehicleOptimalRouteNode.GetOptimalRouteNode(vehicle);
                        OptimalRouteNode vehicleFirstHalf = shortestPath[i][k].GetOptimalRouteNode(vehicle);
                        OptimalRouteNode vehicleSecondHalf = shortestPath[k][j].GetOptimalRouteNode(vehicle);

                        if (vehicleFirstHalf.TimeTakenInMinutes != int.MaxValue && vehicleSecondHalf.TimeTakenInMinutes != int.MaxValue
                            && vehicleOptimal.TimeTakenInMinutes > vehicleFirstHalf.TimeTakenInMinutes + vehicleSecondHalf.TimeTakenInMinutes)
                        {
                            vehicleOptimal.TimeTakenInMinutes = vehicleFirstHalf.TimeTakenInMinutes + vehicleSecondHalf.TimeTakenInMinutes;
                            vehicleOptimal.Route = vehicleFirstHalf.Route.Concat(vehicleSecondHalf.Route).ToList();
                        }
                    }
                }
            }
        }

        return shortestPath;
    }

    public int ComputeTimeTaken(IOrbit orbit, IVehicle vehicle, WeatherConditions weatherCondition, int trafficSpeed)
    {
        int craterReductionPercentage = WeatherFactory.GetCraterChangePercentage(weatherCondition);
        int reducedCrater = Convert.ToInt32((craterReductionPercentage / 100m) * orbit.Craters);

        int totalCraters = orbit.Craters + reducedCrater;
        int vehicleSpeed = Math.Min(vehicle.SpeedInMMPerHr, trafficSpeed);

        int timeTakeninMinutes = vehicle.TimeToCrossCraterInMinutes * totalCraters;
        timeTakeninMinutes += (int)Math.Round(decimal.Divide(orbit.DistanceInMM, vehicleSpeed) * 60);
        return timeTakeninMinutes;
    }
}
}
