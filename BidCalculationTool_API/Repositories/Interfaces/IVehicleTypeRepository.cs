using BidCalculationTool_API.Models;

namespace BidCalculationTool_API.Repositories.Interfaces
{
    public interface IVehicleTypeRepository
    {
        public void InitInMemoryDb();
        Task<IEnumerable<VehicleType>> GetAll();
    }
}
