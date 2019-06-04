using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Extentions;
using Traffic.Implementation;
using Traffic.Interface;

namespace Traffic.Tests
{
    [TestFixture]
    public class ListExtentionsTests
    {
        [Test]
        public void NextPermutationTest1()
        {
            var city1 = new City("city", 1);
            var city2 = new City("city", 2);
            var city3 = new City("city", 3);
            var city4 = new City("city", 4);
            List<ICity> actual = new List<ICity>()
            {
                city1, city2, city3, city4
            };
            List<ICity> expected = new List<ICity>()
            {
                city1, city2, city4, city3
            };
            actual.NextPermutation();
            actual.Should().BeEquivalentTo(expected, m=> m.WithStrictOrdering());
        }
        [Test]
        public void NextPermutationTest2()
        {
            
            var city1 = new City("city", 1);
            var city2 = new City("city", 2);
            var city3 = new City("city", 3);
            var city4 = new City("city", 4);
            List<ICity> actual = new List<ICity>()
            {
                city3, city4, city1, city2
            };
            List<ICity> expected = new List<ICity>()
            {
                city3, city4, city2, city1
            };
            actual.NextPermutation();
            actual.Should().BeEquivalentTo(expected, m => m.WithStrictOrdering());
            actual.NextPermutation();
            expected = new List<ICity>()
            {
                city4, city1, city2, city3
            };
            actual.Should().BeEquivalentTo(expected, m => m.WithStrictOrdering());
        }
        [Test]
        public void NextPermutationTest3()
        {
            ICity hh = new City("Hallitharam", 2);
            ICity rk = new City("RK Puram", 3);
            List<ICity> actual = new List<ICity>()
            {
                rk, hh
            };
            List<ICity> expected = new List<ICity>()
            {
                hh, rk
            };
            actual.NextPermutation();
            actual.Should().BeEquivalentTo(expected, m => m.WithStrictOrdering());
        }
    }
}
