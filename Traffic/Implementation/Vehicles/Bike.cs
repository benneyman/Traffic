using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Enum;
using Traffic.Interface;

namespace Traffic.Implementation.Vehicles
{
    public class Bike : IVehicle
    {
        public int SpeedInMMPerHr { get; private set; }
        public int TimeToCrossCraterInMinutes { get; private set; }
        public WeatherConditions TravellableConditons { get; private set; }
        public VehicleType VehicleType  { get; private set; }

        public Bike(int speed = 10, int timeToCross = 2, WeatherConditions travellableConditions = WeatherConditions.Sunny | WeatherConditions.Windy)
        {
            SpeedInMMPerHr = speed;
            TimeToCrossCraterInMinutes = timeToCross;
            TravellableConditons = travellableConditions;
            VehicleType = VehicleType.Bike;
        }
    }
}
