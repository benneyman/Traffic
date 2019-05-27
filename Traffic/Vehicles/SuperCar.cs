using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Enum;
using Traffic.Interface;

namespace Traffic.Vehicles
{
    public class SuperCar : IVehicle
    {
        public int SpeedInMMPerHr { get; private set; }
        public int TimeToCrossCraterInMinutes { get; private set; }
        public WeatherConditions TravellableConditons { get; private set; }
        public VehicleType VehicleType { get; private set; }

        public SuperCar(int speed = 20, int timeToCross = 3, WeatherConditions travellableConditions = WeatherConditions.Sunny | WeatherConditions.Rainy | WeatherConditions.Windy)
        {
            SpeedInMMPerHr = speed;
            TimeToCrossCraterInMinutes = timeToCross;
            TravellableConditons = travellableConditions;
            VehicleType = VehicleType.SuperCar;
        }
    }
}
