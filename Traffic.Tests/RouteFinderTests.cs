using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Traffic.DTOs;
using Traffic.Enum;
using Traffic.Implementation;
using Traffic.Implementation.Vehicles;
using Traffic.Interface;

namespace Traffic.Tests
{
    [TestFixture]
    public class RouteFinderTests
    {
        IVehiclesProcessor vehiclesProcessor;
        ICitiesGraph citiesGraph;
        List<OrbitCondition> orbitConditions;
        ICity ss = new City("Silk Dorb", 1);
        ICity hh = new City("Hallitharam", 2);
        ICity rk = new City("RK Puram", 3);
        IOrbit orbit1 = new Orbit(1, 1, "Orbit 1");
        IOrbit orbit2 = new Orbit(1, 1, "Orbit 2");
        IOrbit orbit3 = new Orbit(1, 1, "Orbit 3");
        IOrbit orbit4 = new Orbit(1, 1, "Orbit 4");

        [SetUp]
        public void SetUp()
        {
            var vehicles = new List<IVehicle>()
            {
                new Bike(3, 3),
                new TukTuk(2, 2),
                new SuperCar(1, 1)
            };
            vehiclesProcessor = new VehiclesProcessor(vehicles);

            citiesGraph = new CitiesGraph();

            citiesGraph.AddNewRoute(ss, hh, orbit2);
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(ss, rk, orbit3);
            citiesGraph.AddNewRoute(rk, hh, orbit4);

            orbitConditions = new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 20),
                new OrbitCondition(orbit2, 12),
                new OrbitCondition(orbit3, 15),
                new OrbitCondition(orbit4, 12)
            };
        }
        [Test]
        public void TravelTimeComputationTest1()
        {
            var orbit = new Orbit(10, 100, "Test orbit");
            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Sunny))
                .Returns(-10);
            var routeFinder = new RouteFinder(citiesGraph, vehiclesProcessor, weatherMock.Object);
            var vehicle = new Bike();
            int actual = routeFinder.ComputeTimeTaken(orbit, vehicle, WeatherConditions.Sunny, 5);
            actual.Should().Be(300);
        }
        [Test]
        public void TravelTimeComputationTest2()
        {
            var orbit = new Orbit(24, 10, "Test orbit");
            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Windy))
                .Returns(-5);
            var routeFinder = new RouteFinder(citiesGraph, vehiclesProcessor, weatherMock.Object);
            var vehicle = new TukTuk();
            int actual = routeFinder.ComputeTimeTaken(orbit, vehicle, WeatherConditions.Windy, 20);
            actual.Should().Be(130);
        }
        [Test]
        public void TravelTimeComputationTest3()
        {
            var orbit = new Orbit(5, 1, "Test orbit");
            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Windy))
                .Returns(-5);
            var routeFinder = new RouteFinder(citiesGraph, vehiclesProcessor, weatherMock.Object);
            var vehicle = new Bike(60, 1);
            int actual = routeFinder.ComputeTimeTaken(orbit, vehicle, WeatherConditions.Windy, 100);
            actual.Should().Be(6);
        }
        [Test]
        public void ShortestPathForBikeSunnyTest()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(5, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(3, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var vehicle = new Bike(60, 1);
            var allVehicles = new List<IVehicle>() { vehicle };
            ICitiesGraph citiesGraph = new CitiesGraph();
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(hh, rk, orbit2);
            citiesGraph.AddNewRoute(rk, ss, orbit3);
            var vehicleProcessor = new VehiclesProcessor(allVehicles);

            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Sunny))
                .Returns(0);

            var routeFinder = new RouteFinder(citiesGraph, vehicleProcessor, weatherMock.Object);
            IOrbitProcessor orbitProcessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 100),
                new OrbitCondition(orbit2, 100),
                new OrbitCondition(orbit3, 100)
            });
            List<List<AllVehicleOptimalRouteNode>> actual = routeFinder.AllPairShortestPath(WeatherConditions.Sunny, orbitProcessor);
            var expected = new List<List<AllVehicleOptimalRouteNode>>()
            {
                new List<AllVehicleOptimalRouteNode>()
                {
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(ss, ss, 0, new List<IOrbit>()) }
                        }),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(ss, hh, 5, new List<IOrbit>(){ orbit3, orbit2 }) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(ss, rk, 1, new List<IOrbit>(){ orbit3 }) }
                        }
                    )
                },
                new List<AllVehicleOptimalRouteNode>()
                {
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(hh, ss, 5, new List<IOrbit>(){ orbit2, orbit3}) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(hh, hh, 0, new List<IOrbit>()) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(hh, rk, 4, new List<IOrbit>(){ orbit2 }) }
                        }
                    )
                },
                new List<AllVehicleOptimalRouteNode>()
                {
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(rk, ss, 1, new List<IOrbit>(){ orbit3}) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(rk, hh, 4, new List<IOrbit>(){ orbit2 }) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(rk, rk, 0, new List<IOrbit>()) }
                        }
                    )
                }
            };
            actual.Should().BeEquivalentTo(expected, m => m.WithStrictOrdering());
        }
        [Test]
        public void ShortestPathForBikeWindyTest()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(5, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(3, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var vehicle = new Bike(60, 1);
            var allVehicles = new List<IVehicle>() { vehicle };
            ICitiesGraph citiesGraph = new CitiesGraph();
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(hh, rk, orbit2);
            citiesGraph.AddNewRoute(rk, ss, orbit3);
            var vehicleProcessor = new VehiclesProcessor(allVehicles);

            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Windy))
                .Returns(0);

            var routeFinder = new RouteFinder(citiesGraph, vehicleProcessor, weatherMock.Object);
            IOrbitProcessor orbitProcessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 100),
                new OrbitCondition(orbit2, 100),
                new OrbitCondition(orbit3, 100)
            });
            List<List<AllVehicleOptimalRouteNode>> actual = routeFinder.AllPairShortestPath(WeatherConditions.Windy, orbitProcessor);
            var expected = new List<List<AllVehicleOptimalRouteNode>>()
            {
                new List<AllVehicleOptimalRouteNode>()
                {
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(ss, ss, 0, new List<IOrbit>()) }
                        }),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(ss, hh, 5, new List<IOrbit>(){ orbit3, orbit2 }) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(ss, rk, 1, new List<IOrbit>(){ orbit3 }) }
                        }
                    )
                },
                new List<AllVehicleOptimalRouteNode>()
                {
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(hh, ss, 5, new List<IOrbit>(){ orbit2, orbit3}) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(hh, hh, 0, new List<IOrbit>()) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(hh, rk, 4, new List<IOrbit>(){ orbit2 }) }
                        }
                    )
                },
                new List<AllVehicleOptimalRouteNode>()
                {
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(rk, ss, 1, new List<IOrbit>(){ orbit3}) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(rk, hh, 4, new List<IOrbit>(){ orbit2 }) }
                        }
                    ),
                    new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
                        {
                            { vehicle, new OptimalRouteNode(rk, rk, 0, new List<IOrbit>()) }
                        }
                    )
                }
            };
            actual.Should().BeEquivalentTo(expected, m => m.WithStrictOrdering());
        }
        [Test]
        public void OptimalRouteTestWithoutStops()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(2, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(7, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var vehicle = new Bike(60, 1);
            var allVehicles = new List<IVehicle>() { vehicle };
            ICitiesGraph citiesGraph = new CitiesGraph();
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(hh, rk, orbit2);
            citiesGraph.AddNewRoute(rk, ss, orbit3);
            var vehicleProcessor = new VehiclesProcessor(allVehicles);

            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Windy))
                .Returns(0);

            var routeFinder = new RouteFinder(citiesGraph, vehicleProcessor, weatherMock.Object);
            IOrbitProcessor orbitProcessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 100),
                new OrbitCondition(orbit2, 100),
                new OrbitCondition(orbit3, 100)
            });

            var actual = routeFinder.OptimalRoute(orbitProcessor, WeatherConditions.Windy, ss, new List<ICity>() { hh });
            var expected = new OptimalPathAndVehicle(vehicle, new List<OptimalRouteNode>()
            {
                new OptimalRouteNode(ss, hh, 4, new List<IOrbit>() { orbit1 }) 
            }, 4);
            actual.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void OptimalRouteTestWithStops1()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(2, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(7, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var vehicle = new Bike(60, 1);
            var allVehicles = new List<IVehicle>() { vehicle };
            ICitiesGraph citiesGraph = new CitiesGraph();
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(hh, rk, orbit2);
            citiesGraph.AddNewRoute(rk, ss, orbit3);
            var vehicleProcessor = new VehiclesProcessor(allVehicles);

            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Sunny))
                .Returns(0);

            var routeFinder = new RouteFinder(citiesGraph, vehicleProcessor, weatherMock.Object);
            IOrbitProcessor orbitProcessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 100),
                new OrbitCondition(orbit2, 100),
                new OrbitCondition(orbit3, 100)
            });

            var actual = routeFinder.OptimalRoute(orbitProcessor, WeatherConditions.Sunny, ss, new List<ICity>() { rk, hh });
            var expected = new OptimalPathAndVehicle(vehicle, new List<OptimalRouteNode>() {
                new OptimalRouteNode (ss, rk, 1, new List<IOrbit>() { orbit3 }),
                new OptimalRouteNode (rk, hh, 5, new List<IOrbit>() { orbit3, orbit1 })
            }, 6);
            actual.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void OptimalRouteTestWithStops2()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(2, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(7, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var vehicle = new Bike(60, 1);
            var allVehicles = new List<IVehicle>() { vehicle };
            ICitiesGraph citiesGraph = new CitiesGraph();
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(hh, rk, orbit2);
            citiesGraph.AddNewRoute(rk, ss, orbit3);
            var vehicleProcessor = new VehiclesProcessor(allVehicles);

            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Windy))
                .Returns(0);

            var routeFinder = new RouteFinder(citiesGraph, vehicleProcessor, weatherMock.Object);
            IOrbitProcessor orbitProcessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 100),
                new OrbitCondition(orbit2, 100),
                new OrbitCondition(orbit3, 100)
            });

            var actual = routeFinder.OptimalRoute(orbitProcessor, WeatherConditions.Windy, rk, new List<ICity>() { ss, hh });
            var expected = new OptimalPathAndVehicle(vehicle, new List<OptimalRouteNode>()
            {
                new OptimalRouteNode(rk, ss, 1, new List<IOrbit>(){ orbit3 }),
                new OptimalRouteNode(ss, hh, 4, new List<IOrbit>(){ orbit1 })
            }, 5);
            actual.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void OptimalRouteTestWithStops3()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(2, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(7, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var vehicle = new Bike(60, 1);
            var allVehicles = new List<IVehicle>() { vehicle };
            ICitiesGraph citiesGraph = new CitiesGraph();
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(hh, rk, orbit2);
            citiesGraph.AddNewRoute(rk, ss, orbit3);
            var vehicleProcessor = new VehiclesProcessor(allVehicles);

            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Windy))
                .Returns(0);

            var routeFinder = new RouteFinder(citiesGraph, vehicleProcessor, weatherMock.Object);
            IOrbitProcessor orbitProcessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 100),
                new OrbitCondition(orbit2, 100),
                new OrbitCondition(orbit3, 100)
            });

            var actual = routeFinder.OptimalRoute(orbitProcessor, WeatherConditions.Windy, ss, new List<ICity>() { hh, rk });
            var expected = new OptimalPathAndVehicle(vehicle, new List<OptimalRouteNode>() {
                new OptimalRouteNode (ss, rk, 1, new List<IOrbit>() { orbit3 }),
                new OptimalRouteNode (rk, hh, 5, new List<IOrbit>() { orbit3, orbit1 })
            }, 6);
            actual.Should().BeEquivalentTo(expected, m =>  m.WithStrictOrdering());
        }
        [Test]
        public void OptimalRouteTestWithStops4()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(2, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(7, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var vehicle = new Bike(60, 1);
            var allVehicles = new List<IVehicle>() { vehicle };
            ICitiesGraph citiesGraph = new CitiesGraph();
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(hh, rk, orbit2);
            citiesGraph.AddNewRoute(rk, ss, orbit3);
            var vehicleProcessor = new VehiclesProcessor(allVehicles);

            var weatherMock = new Mock<IWeatherFactory>();
            weatherMock.Setup(m => m.GetCraterChangePercentage(WeatherConditions.Windy))
                .Returns(0);

            var routeFinder = new RouteFinder(citiesGraph, vehicleProcessor, weatherMock.Object);
            IOrbitProcessor orbitProcessor = new OrbitProcessor(new List<OrbitCondition>()
            {
                new OrbitCondition(orbit1, 100),
                new OrbitCondition(orbit2, 100),
                new OrbitCondition(orbit3, 100)
            });

            var actual = routeFinder.OptimalRoute(orbitProcessor, WeatherConditions.Windy, hh, new List<ICity>() { ss, rk });
            var expected = new OptimalPathAndVehicle(vehicle, new List<OptimalRouteNode>()
            {
                new OptimalRouteNode(hh, ss, 4, new List<IOrbit>(){ orbit1 }),
                new OptimalRouteNode(ss, rk, 1, new List<IOrbit>(){ orbit3 })
            }, 5);
            actual.Should().BeEquivalentTo(expected, m=> m.WithStrictOrdering());
        }


    }
}
