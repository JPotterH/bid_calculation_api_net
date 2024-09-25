using BidCalculationTool_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BidCalculationTool_API.Context
{
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