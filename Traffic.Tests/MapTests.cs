using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.Implementation;
using Traffic.Interface;

namespace Traffic.Tests
{
    [TestFixture]
    class MapTests
    {
        [Test]
        public void CheckIfEdgesAddedCorrectly()
        {
            CitiesGraph map = new CitiesGraph();
            var fromCity = new City("Silk", 1);
            var toCity = new City("bulk", 2);
            var orbit = new Orbit(1, 1, "orbit 1");
            map.AddNewRoute(fromCity, toCity, orbit);

            var actual = map.GetRoutesFrom(fromCity);
            var expected = new List<Edge>() { new Edge(toCity, orbit) };
            actual.Should().BeEquivalentTo(expected);
            map.TotalCities.Should().Be(2);

        }

        [Test]
        public void CheckIfTotalCitiesAreCorrect()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            IOrbit orbit1 = new Orbit(18, 20, "Orbit 1");
            IOrbit orbit2 = new Orbit(20, 10, "Orbit 2");
            IOrbit orbit3 = new Orbit(30, 15, "Orbit 3");
            IOrbit orbit4 = new Orbit(15, 18, "Orbit 4");

            ICitiesGraph citiesGraph = new CitiesGraph();

            citiesGraph.AddNewRoute(ss, hh, orbit2);
            citiesGraph.AddNewRoute(ss, hh, orbit1);
            citiesGraph.AddNewRoute(ss, rk, orbit3);
            citiesGraph.AddNewRoute(rk, hh, orbit4);

            citiesGraph.TotalCities.Should().Be(3);
        }

    }
}
