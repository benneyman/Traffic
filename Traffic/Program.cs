using System;
using System.Collections.Generic;
using Traffic.DTOs;
using Traffic.Interface;
using Traffic.Enum;
using Traffic.Implementation;
using Traffic.Factories;
using System.Linq;
using System.IO;

namespace Traffic
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IVehicle> vehicles = new List<IVehicle>()
            {
                VehicleFactory.GetVehicle(VehicleType.Bike),
                VehicleFactory.GetVehicle(VehicleType.TukTuk),
                VehicleFactory.GetVehicle(VehicleType.SuperCar)
            };
            IVehiclesProcessor vehiclesProcessor = new VehiclesProcessor(vehicles);
            var files = Directory.GetFiles(@"~\Input", "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                Console.WriteLine($"Executing {Path.GetFileNameWithoutExtension(file)}");
                var cities = new Dictionary<string, City>();
                var orbits = new Dictionary<string, Orbit>();
                using (var reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        ICitiesGraph citiesGraph = new CitiesGraph();
                        int n, o, e, t;
                        var values = reader.ReadLine().Split(",").Select(m => m.Trim()).ToList();
                        n = Convert.ToInt16(values[0]);
                        o = Convert.ToInt16(values[1]);
                        e = Convert.ToInt16(values[2]);
                        t = Convert.ToInt16(values[3]);

                        var cityNames = reader.ReadLine().Split(",").Select(m => m.Trim()).ToList();
                        //Read cities
                        for (int i = 0; i < n; i++)
                        {
                            var city = new City(cityNames[i], i + 1);
                            cities.Add(cityNames[i], city);
                        }
                        //Read Orbits
                        for (int i = 0; i < o; i++)
                        {
                            var orbit = reader.ReadLine().Split(",").Select(m => m.Trim()).ToList();
                            int distance = Convert.ToInt32(orbit[0]);
                            int craters = Convert.ToInt32(orbit[1]);
                            string orbitName = orbit[2];
                            orbits.Add(orbitName, new Orbit(distance, craters, orbitName));
                        }
                        //Read Edges
                        for (int i = 0; i < e; i++)
                        {
                            var edge = reader.ReadLine().Split(",").Select(m => m.Trim()).ToList();
                            string source = edge[0];
                            string target = edge[1];
                            string orbit = edge[2];
                            citiesGraph.AddNewRoute(cities[source], cities[target], orbits[orbit]);
                        }
                        IRouteFinder routeFinder = new RouteFinder(citiesGraph, vehiclesProcessor, new WeatherFactory());

                        //Test Cases
                        for (int i = 0; i < t; i++)
                        {
                            Console.WriteLine($"TestCase {i + 1}");
                            var orbitConditions = new List<OrbitCondition>();
                            var wCondition = reader.ReadLine().Trim();
                            WeatherConditions weather;
                            switch (wCondition)
                            {
                                case "Windy":
                                    weather = WeatherConditions.Windy;
                                    break;
                                case "Sunny":
                                    weather = WeatherConditions.Sunny;
                                    break;
                                case "Rainy":
                                    weather = WeatherConditions.Rainy;
                                    break;
                                default:
                                    weather = WeatherConditions.Sunny;
                                    break;
                            }
                            Console.WriteLine($"Input: Weather is {weather}");
                            for (int j = 0; j < o; j++)
                            {
                                var orbit = reader.ReadLine().Split(",").Select(m => m.Trim()).ToList();
                                string orbitName = orbit[0];
                                int speed = Convert.ToInt16(orbit[1]);
                                orbitConditions.Add(new OrbitCondition(orbits[orbitName], speed));
                                Console.WriteLine($"Input: {orbitName} speed is {speed} megamiles/hour");
                            }
                            var orbitProccessor = new OrbitProcessor(orbitConditions);
                            List<ICity> inputCities = reader.ReadLine().Split(",").Select(m => cities[m.Trim()]).ToList<ICity>();
                            OptimalPathAndVehicle optimalPath = routeFinder.OptimalRoute(orbitProccessor, weather, inputCities[0], inputCities.Skip(1).ToList());
                            PrintOutput(optimalPath);
                        }
                    }
                }

            }
            Console.ReadKey();
        }

        static void PrintOutput(OptimalPathAndVehicle optimalPathAndVehicle)
        {
            Console.WriteLine("Output");
            Console.WriteLine($"Vehicle => {optimalPathAndVehicle.Vehicle.VehicleType.ToString()}");
            Console.WriteLine($"Total Cost => {optimalPathAndVehicle.TimeTaken}");
            foreach (OptimalRouteNode optimalRouteNode in optimalPathAndVehicle.Route)
            {
                Console.Write($"{optimalRouteNode.FromCity.Name} to {optimalRouteNode.ToCity.Name} via ");
                for (int i = 0; i < optimalRouteNode.Route.Count; i++)
                {
                    Console.Write($"{ optimalRouteNode.Route[i].Name } ");
                    if (i < optimalRouteNode.Route.Count - 1)
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