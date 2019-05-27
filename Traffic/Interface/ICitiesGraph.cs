using System.Collections.Generic;
using Traffic.Interface;

namespace Traffic.Interface
{
    public interface ICitiesGraph
    {
        Dictionary<ICity, List<IEdge>> CitiesMap { get; }
        int TotalCities { get; }

        void AddNewRoute(ICity from, ICity to, IOrbit orbit);
        List<IEdge> GetRoutesFrom(ICity from);
    }
}