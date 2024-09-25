using BidCalculationTool_API.Models;
using BidCalculationTool_API.Repositories.Interfaces;
using BidCalculationTool_API.Services.Interfaces;

namespace BidCalculationTool_API.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        protected readonly IVehicleTypeRepository _vehicleTypeRepository;

        public VehicleTypeService(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<IEnumerable<VehicleType>> GetAll()
        {
            var vehicleTypes = await _vehicleTypeRepository.GetAll();
            return vehicleTypes;
        }
    }
}
