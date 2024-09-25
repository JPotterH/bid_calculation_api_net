using BidCalculationTool_API.Context;
using BidCalculationTool_API.Models;
using BidCalculationTool_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BidCalculationTool_API.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly InMemoryContext _context;

        public VehicleRepository(InMemoryContext inMemmoryContext)
        {
            _context = inMemmoryContext;
        }

        // Seeding of the in memory DB, for other contexts could be done directly on the context
        public void InitInMemoryDb()
        {
            var vehicles = new List<Vehicle>
                {
                new() {
                    Id = 1,
                    BasePrice = 397.75m,
                    Description = "Spare parts vehicle",
                    Type = (int)VehicleTypeEnum.Common
                },
                new ()
                {
                    Id = 2,
                    BasePrice = 501,
                    Description = "Accidented common vehicle",
                    Type = (int)VehicleTypeEnum.Common
                },
                new ()
                {
                    Id = 3,
                    BasePrice = 57,
                    Description = "Popeye's toy vehicle",
                    Type = (int)VehicleTypeEnum.Common
                },
                new ()
                {
                    Id = 4,
                    BasePrice = 1800,
                    Description = "Partially damaged luxury vehicle",
                    Type = (int)VehicleTypeEnum.Luxury
                },
                new ()
                {
                    Id = 5,
                    BasePrice = 1100,
                    Description = "Second hand common vehicle",
                    Type = (int)VehicleTypeEnum.Common
                },
                new ()
                {
                    Id = 6,
                    BasePrice = 1000000,
                    Description = "Mint condition luxury vehicle",
                    Type = (int)VehicleTypeEnum.Luxury
                }
            };

            _context.Vehicles.AddRange(vehicles);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetById(int id)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return await _context.Vehicles.AsNoTracking().SingleOrDefaultAsync(vehicle => vehicle.Id == id);
            // Possible null return is left to be handled by the controller to help provide the user with more accurate response
            #pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
