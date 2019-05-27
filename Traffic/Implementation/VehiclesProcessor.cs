using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.Enum;
using Traffic.Interface;

namespace Traffic.Implementation
{
    public class VehiclesProcessor : IVehiclesProcessor
    {
        private Dictionary<VehicleType, IVehicle> _allVehicle;
        public  List<IVehicle> Vehicles { get; }

        public VehiclesProcessor(List<IVehicle> vehicles)
        {
            _allVehicle = vehicles.ToDictionary(key => key.VehicleType, value => value);
            Vehicles = vehicles;
        }

        public IVehicle GetVehicleFromType(VehicleType vehicleType)
        {
            IVehicle vehicle;
            if (!_allVehicle.TryGetValue(vehicleType, out vehicle))
                throw new KeyNotFoundException($"{vehicle.VehicleType} not found");
            return vehicle;
        }
        public List<IVehicle> GetVehiclesFromType(List<VehicleType> vehicleTypes)
        {
            try
            {
                List<IVehicle> vehicles = new List<IVehicle>();
                foreach (var vechicleType in vehicleTypes)
                {
                    vehicles.Add(GetVehicleFromType(vechicleType));
                }
                return vehicles;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }
    }
}
