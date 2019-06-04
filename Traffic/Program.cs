using System;
using System.Collections.Generic;
using Traffic.DTOs;
using Traffic.Interface;
using Traffic.Enum;
using Traffic.Implementation;
using Traffic.Implementation.Vehicles;
using Traffic.Factories;

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

            IOrbit orbit1 = new Orbit(18, 20, "Orbit 1");
            IOrbit orbit2 = new Orbit(20, 10, "Orbit 2");
            IOrbit orbit3 = new Orbit(30, 15, "Orbit 3");
            IOrbit orbit4 = new Orbit(15, 18, "Orbit 4");

            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);

            ICitiesGraph citiesGraph = new CitiesGraph();

            citiesGraph.AddNewRoute(ss, hh, orbit2);
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(ss, rk, orbit3);
            citiesGraph.AddNewRoute(rk, hh, orbit4);

            IRouteFinder routeFinder = new RouteFinder(citiesGraph, vehiclesProcessor, new WeatherFactory());
            IOrbitProcessor orbitProccessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 20),
                new OrbitCondition(orbit2, 12),
                new OrbitCondition(orbit3, 15),
                new OrbitCondition(orbit4, 12)
            });
            OptimalPathAndVehicle optimalPath = routeFinder.OptimalRoute(orbitProccessor, WeatherConditions.Sunny, ss, new List<ICity>() { rk, hh });
            PrintOutput(optimalPath);
            orbitProccessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 5),
                new OrbitCondition(orbit2, 10),
                new OrbitCondition(orbit3, 20),
                new OrbitCondition(orbit4, 20)
            });
            optimalPath = routeFinder.OptimalRoute(orbitProccessor, WeatherConditions.Windy, ss, new List<ICity>() { rk, hh });
            PrintOutput(optimalPath);
            Console.ReadKey();


        }

        static void PrintOutput(OptimalPathAndVehicle optimalPathAndVehicle)
        {
            Console.WriteLine($"Vehicle => {optimalPathAndVehicle.Vehicle.VehicleType.ToString()}");
            Console.WriteLine($"Total Cost => {optimalPathAndVehicle.TimeTaken}");
            foreach (OptimalRouteNode optimalRouteNode in optimalPathAndVehicle.Route)
            {
                Console.Write($"{optimalRouteNode.FromCity.Name} to {optimalRouteNode.ToCity.Name} via ");
                for (int i = 0; i < optimalRouteNode.Route.Count; i++)
                {
                    Console.Write($"{ optimalRouteNode.Route[i].Name } ");
                    if(i < optimalRouteNode.Route.Count - 1)
                    {
                        Console.Write(" => ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}