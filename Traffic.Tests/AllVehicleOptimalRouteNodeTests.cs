using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Traffic.DTOs;
using Traffic.Enum;
using Traffic.Factories;
using Traffic.Implementation;
using Traffic.Interface;

namespace Traffic.Tests
{
    [TestFixture]
    class AllVehicleOptimalRouteNodeTests
    {
        [Test]
        public void OperatorPlusOverloadTest()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(5, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(3, 1, "Orbit 2");
            IOrbit orbit3 = new Orbit(1, 0, "Orbit 3");

            var bike = new Vehicle(60, 1, WeatherConditions.Sunny | WeatherConditions.Windy, VehicleType.Bike);
            var tk = VehicleFactory.GetVehicle(VehicleType.TukTuk);

            var first = new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
            {
                { bike, new OptimalRouteNode(ss, hh, 10, new List<IOrbit>(){ orbit1 }) },
                { tk, new OptimalRouteNode(ss, hh, 20, new List<IOrbit>(){ orbit2, orbit3 }) }
            });
            var second = new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
            {
                { bike, new OptimalRouteNode(hh, rk, 10, new List<IOrbit>(){ orbit3 }) },
                { tk, new OptimalRouteNode(hh, rk, 20, new List<IOrbit>(){ orbit1}) }
            });

            var actual = first + second;
            var expected = new AllVehicleOptimalRouteNode(new Dictionary<IVehicle, OptimalRouteNode>()
            {
                { bike, new OptimalRouteNode(ss, rk, 20, new List<IOrbit>(){ orbit1, orbit3 }) },
                { tk, new OptimalRouteNode(ss, rk, 40, new List<IOrbit>(){ orbit2, orbit3, orbit1 }) }
            });

            actual.Should().BeEquivalentTo(expected, m => m.WithStrictOrdering());
        }
    }
}
