using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Enum;
using Traffic.Interface;

namespace Traffic.Vehicles
{
    public class TukTuk : IVehicle
    {
        public int SpeedInMMPerHr { get; private set; }
        public int TimeToCrossCraterInMinutes { get; private set; }
        public WeatherConditions TravellableConditons { get; private set; }
        public VehicleType VehicleType { get; private set; }

        public TukTuk(int speed = 12, int timeToCross = 1, WeatherConditions travellableConditions = WeatherConditions.Sunny | WeatherConditions.Rainy)
        {
            SpeedInMMPerHr = speed;
            TimeToCrossCraterInMinutes = timeToCross;
            TravellableConditons = travellableConditions;
            VehicleType = VehicleType.TukTuk;
        }
    }
}
