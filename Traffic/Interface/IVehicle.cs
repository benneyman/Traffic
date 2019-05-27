using Traffic.Enum;

namespace Traffic.Interface
{
    public interface IVehicle
    {
        int SpeedInMMPerHr { get; }
        int TimeToCrossCraterInMinutes { get; }
        WeatherConditions TravellableConditons { get;  }
        VehicleType VehicleType { get; }
    }
}