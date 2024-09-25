using BidCalculationTool_API.Models;

namespace BidCalculationTool_API.Services.Interfaces
{
    public interface IVehicleTypeService
    {
        Task<IEnumerable<VehicleType>> GetAll();
    }
}
