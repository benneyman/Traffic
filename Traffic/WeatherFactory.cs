using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Enum;

namespace Traffic
{
    public static class WeatherFactory
    {
        public static int GetCraterReductionPercentage(WeatherConditions weatherCondition)
        {
            switch (weatherCondition)
            {
                case WeatherConditions.Sunny:
                    return -10;
                case WeatherConditions.Windy:
                    return 0;
                case WeatherConditions.Rainy:
                    return 20;
                default:
                    return 0;
            }
        }
    }
}
