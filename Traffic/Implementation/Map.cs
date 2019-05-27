using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class CitiesGraph : ICitiesGraph
    {
        public Dictionary<ICity, List<IEdge>> CitiesMap { get; private set; }
        public int TotalCities { get; private set; }

        public CitiesGraph()
        {
            CitiesMap = new Dictionary<ICity, List<IEdge>>();
            TotalCities = 0;
        }

        public void AddNewRoute(ICity from, ICity to, IOrbit orbit)
        {
            List<IEdge> edges = new List<IEdge>();

            Edge toEdge = new Edge(to, orbit);
            Edge fromEdge = new Edge(from, orbit);

            if (CitiesMap.TryGetValue(from, out edges))
            {
                edges.Add(toEdge);
            }
            else
            {
                CitiesMap.Add(from, new List<IEdge>() { toEdge });
                TotalCities += 1;
            }
            if (CitiesMap.TryGetValue(to, out edges))
            {
                edges.Add(fromEdge);
            }
            else
            {
                CitiesMap.Add(to, new List<IEdge>() { fromEdge });
                TotalCities += 1;
            }
        }

        public List<IEdge> GetRoutesFrom(ICity from)
        {
            List<IEdge> edges = new List<IEdge>();
            CitiesMap.TryGetValue(from, out edges);
            return edges;
        }
    }
}
