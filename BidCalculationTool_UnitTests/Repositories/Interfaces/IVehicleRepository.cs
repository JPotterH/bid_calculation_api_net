using BidCalculationTool_API.Models;

namespace BidCalculationTool_UnitTests.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        public void InitInMemoryDb();
        Task<IEnumerable<Vehicle>> GetAll();
        Task<Vehicle> GetById(int id);
    }
}
