using System;
using System.Collections.Generic;
using System.Text;
using Traffic.Enum;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class Vehicle : IVehicle
    {
        public int SpeedInMMPerHr { get; private set; }
        public int TimeToCrossCraterInMinutes { get; private set; }
        public WeatherConditions TravellableConditons { get; private set; }
        public VehicleType VehicleType { get; private set; }

        public Vehicle(int speed, int timeToCross, WeatherConditions travellableConditions, VehicleType vehicleType)
        {
            SpeedInMMPerHr = speed;
            TimeToCrossCraterInMinutes = timeToCross;
            TravellableConditons = travellableConditions;
            VehicleType = vehicleType;
        }
    }
}
