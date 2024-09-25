using BidCalculationTool_API.Models;
using BidCalculationTool_API.Repositories.Interfaces;
using BidCalculationTool_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BidCalculationTool_API.Services
{
    public class VehicleService : IVehicleService
    {
        protected readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            var vehicles = await _vehicleRepository.GetAll();
            return vehicles;
        }

        public async Task<Vehicle> GetById(int id)
        {
            var vehicle = await _vehicleRepository.GetById(id);
            return vehicle;
        }
    }
}
