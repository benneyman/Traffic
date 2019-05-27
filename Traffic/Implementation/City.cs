using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class City : ICity
    {
        public City(string name, int Id)
        {
            Name = name;
            CityId = Id;
        }

        public string Name { get; }
        public int CityId { get; }
    }
}
