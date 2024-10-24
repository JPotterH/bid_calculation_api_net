using BidCalculationTool_API.Models;
using BidCalculationTool_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidCalculationTool_UnitTests.Services
{
    internal class VehicleTestService : IVehicleService
    {
        private readonly List<Vehicle> _vehicleList;

        public VehicleTestService()
        {
            _vehicleList = new List<Vehicle>()
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
                    BasePrice = 57000.45m,
                    Description = "Popeye's toy vehicle",
                    Type = (int)VehicleTypeEnum.Luxury
                }
            };
        }

        Task<IEnumerable<Vehicle>> IVehicleService.GetAll()
        {
            return Task.FromResult<IEnumerable<Vehicle>>(_vehicleList);
        }

        async Task<Vehicle> IVehicleService.GetById(int id)
        {
            var vehicle = _vehicleList.Where(v => v.Id == id).FirstOrDefault()!;
            return await Task.FromResult(vehicle);
        }
    }
}
