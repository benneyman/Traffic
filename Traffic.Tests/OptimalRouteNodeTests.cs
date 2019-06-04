using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Traffic.DTOs;
using Traffic.Implementation;
using Traffic.Interface;

namespace Traffic.Tests
{
    [TestFixture]
    public class OptimalRouteNodeTests
    {
        [Test]
        public void OperatorPlusOverloadingTest()
        {
            ICity ss = new City("Silk Dorb", 1);
            ICity hh = new City("Hallitharam", 2);

            IOrbit orbit1 = new Orbit(5, 2, "Orbit 1");
            IOrbit orbit2 = new Orbit(3, 1, "Orbit 2");

            var first = new OptimalRouteNode(ss, hh, 100, new List<IOrbit>() { orbit1 });
            var second = new OptimalRouteNode(ss, hh, 250, new List<IOrbit>() { orbit2 });

            var actual = first + second;
            var expected = new OptimalRouteNode(ss, hh,350, new List<IOrbit>() { orbit1, orbit2 });
            actual.Should().BeEquivalentTo(expected);
        }

    }
}
