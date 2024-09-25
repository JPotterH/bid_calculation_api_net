using BidCalculationTool_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BidCalculationTool_API.Context
{
    /*
     * InMemory might be the worst option to go with for testing purposes
     * Chosen based on the implicity of the data storage requirements and
     * as the less coniguration requiring option.
     * 
     * Once the project gets green light, DB context structure is already
     * in place and it's just a matter of replacing current context by a
     * better one.
     */
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options)
        : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<VehicleType> VehicleTypes { get; set; } = null!;
    }
}