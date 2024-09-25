using BidCalculationTool_API.Models;

namespace BidCalculationTool_API.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAll();
        Task<Vehicle> GetById(int id);
    }
}
