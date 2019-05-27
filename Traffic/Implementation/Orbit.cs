using System;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class Orbit : IOrbit
    {
        public int DistanceInMM { get; private set; }
        public int Craters { get; private set; }
        public string Name { get; private set; }

        public Orbit(int distanceInMM, int craters, string name)
        {
            DistanceInMM = distanceInMM;
            Craters = craters;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

    }
}