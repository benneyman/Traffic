using System.Collections.Generic;
using Traffic.Enum;
using Traffic.Interface;

namespace Traffic.Interface
{
    public interface IVehiclesProcessor
    {
        List<IVehicle> Vehicles { get; }

        IVehicle GetVehicleFromType(VehicleType vehicleType);
        List<IVehicle> GetVehiclesFromType(List<VehicleType> vehicleTypes);
    }
}