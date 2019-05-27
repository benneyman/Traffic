using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Enum
{
    [Flags]
    public enum WeatherConditions
    {
        Sunny = 1 << 0,
        Windy = 1 << 1,
        Rainy = 1 << 2
    }
}
