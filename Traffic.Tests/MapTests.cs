using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.Implementation;

namespace Traffic.Tests
{
    public class DTO
    {
        public int Cost { get; set; }
        public List<int> Routes { get; set; }
        public DTO(int cost, List<int> routes = null)
        {
            Cost = cost;
            Routes = routes ?? new List<int>();
        }
    }
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
        public void FW()
        {
            object value = null;
            value.ToString();
            var graph = new List<List<Tuple<int, int>>>()
            {
                new List<Tuple<int, int>>() {new Tuple<int, int>(3, 1)},
                new List<Tuple<int, int>>() { new Tuple<int, int>(3, 4), new Tuple<int, int>(4, 2)},
                new List<Tuple<int, int>>() { new Tuple<int, int>(1, 1), new Tuple<int, int>(5, 3), new Tuple<int, int>(2, 4)},
                new List<Tuple<int, int>>() { new Tuple<int, int>(5, 17), new Tuple<int, int>(2, 2)},
                new List<Tuple<int, int>>() { new Tuple<int, int>(3, 3), new Tuple<int, int>(4, 17)},
            };
            
            var dp = new List<List<DTO>>();
            foreach (var i in Enumerable.Range(0, graph.Count))
            {
                dp.Add(new List<DTO>());
                foreach (var j in Enumerable.Range(0, graph.Count)) 
                {
                    DTO dto;
                    if(i == j)
                    {
                        dto = new DTO(0);
                    }
                    else
                    {
                        dto = new DTO(int.MaxValue);
                    }
                    dp[i].Add(dto);
                }
            }
            //Initialize
            for (int i = 0; i < graph.Count; i++)
            {
                foreach (var edge in graph[i])
                {
                    dp[i][edge.Item1 - 1].Cost= edge.Item2;
                    dp[i][edge.Item1 - 1].Routes.Add(i + 1);
                }
            }
            //FW
            for (int k = 0; k < graph.Count; k++)
            {
                for (int i = 0; i < graph.Count; i++)
                {
                    for (int j = 0; j < graph.Count; j++)
                    {
                        if(dp[i][k].Cost != int.MaxValue && dp[k][j].Cost != int.MaxValue && dp[i][k].Cost + dp[k][j].Cost < dp[i][j].Cost)
                        {
                            dp[i][j].Cost =  dp[i][k].Cost + dp[k][j].Cost;
                            dp[i][j].Routes = dp[i][k].Routes.Concat(dp[k][j].Routes).ToList();
                        }
                    }
                }
            }

        }

    }
}
