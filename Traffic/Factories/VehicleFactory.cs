using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Enum;
using Traffic.Implementation;
using Traffic.Interface;

namespace Traffic.Factories
{
    public static class VehicleFactory
    {
        public static IVehicle GetVehicle(VehicleType vehicleType)
        {
            switch (vehicleType)
            {
                case VehicleType.Bike:
                    return new Vehicle(10, 2, WeatherConditions.Sunny | WeatherConditions.Windy, VehicleType.Bike);
                case VehicleType.TukTuk:
                    return new Vehicle(12, 1, WeatherConditions.Sunny | WeatherConditions.Rainy, VehicleType.TukTuk);
                case VehicleType.SuperCar:
                    return new Vehicle(20, 3, WeatherConditions.Sunny | WeatherConditions.Rainy | WeatherConditions.Windy, VehicleType.SuperCar);
                default:
                    throw new InvalidOperationException($"{vehicleType} isn't present.");
            }
        }
    }
}
