using System;
using System.Collections.Generic;
using Traffic.DTOs;
using Traffic.Interface;
using Traffic.Vehicles;
using Traffic.Enum;
using Traffic.Implementation;

namespace Traffic
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IVehicle> vehicles = new List<IVehicle>()
            {
                new Bike(),
                new TukTuk(),
                new SuperCar()
            };
            IVehiclesProcessor vehiclesProcessor = new VehiclesProcessor(vehicles);

            List<IOrbit> orbits = new List<IOrbit>()
            {
                new Orbit(20, 10, "Orbit 1"),
                new Orbit(18, 20, "Orbit 2")
            };
            IOrbitProcessor orbitProcessor = new OrbitProcessor(orbits);

            RouteFinder routeFinder = new RouteFinder(orbitProcessor, vehiclesProcessor);

            OptimalPathAndVehicle result = routeFinder.OptimalRoute(
                new List<OrbitCondition>()
                {
                    new OrbitCondition("Orbit 1", 14),
                    new OrbitCondition("Orbit 2", 20)
                }, WeatherConditions.Windy);

            Console.ReadKey();


        }
    }
}
