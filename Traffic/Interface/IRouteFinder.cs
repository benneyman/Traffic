using System.Collections.Generic;
using Traffic.DTOs;
using Traffic.Enum;
using Traffic.Implementation;

namespace Traffic.Interface
{
    public interface IRouteFinder
    {
        ICitiesGraph CitiesGraph { get; }
        IVehiclesProcessor VehiclesProcessor { get; }

        List<List<AllVehicleOptimalRouteNode>> AllPairShortestPath(WeatherConditions weatherCondition, IOrbitProcessor orbitProcessor);
        OptimalPathAndVehicle OptimalRoute(IOrbitProcessor orbitProcessor, WeatherConditions weatherCondition, ICity startCity, List<ICity> stops);
    }
}