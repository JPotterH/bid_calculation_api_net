using BidCalculationTool_API.Context;
using BidCalculationTool_API.Models;
using BidCalculationTool_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BidCalculationTool_API.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly InMemoryContext _context;

        public VehicleTypeRepository(InMemoryContext inMemmoryContext)
        {
            _context = inMemmoryContext;
        }

        // Seeding of the in memory DB, for other contexts could be done directly on the context
        public void InitInMemoryDb()
        {
            var vehicleTypes = Enum.GetValues(typeof(VehicleTypeEnum))
                .Cast<VehicleTypeEnum>()
                .Select(vt => new VehicleType()
                {
                    Id = (int)vt,
                    Type = vt.ToString()
                }).ToList();

            _context.VehicleTypes.AddRange(vehicleTypes);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<VehicleType>> GetAll()
        {
            return await _context.VehicleTypes.AsNoTracking().ToListAsync();
        }
    }
}
