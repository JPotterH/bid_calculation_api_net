using BidCalculationTool_API.Controllers;
using BidCalculationTool_API.Models;
using BidCalculationTool_API.Services.Interfaces;
using BidCalculationTool_UnitTests.Services;
using Microsoft.AspNetCore.Mvc;

namespace BidCalculationTool_UnitTests.UnitTests
{
    public class VehicleUnitTest
    {
        private readonly IVehicleService _vehicleService;
        private readonly VehicleController _controller;

        public VehicleUnitTest()
        {
            _vehicleService = new VehicleTestService();
            _controller = new VehicleController(_vehicleService);
        }

        // Warming up
        [Fact]
        public async void Test_GetVehiclesInfo_ReturnsOkResult()
        {
            // Act
            var result = await _controller.GetVehiclesInfo();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result as OkObjectResult);
        }

        [Fact]
        public async void Test_GetVehiclesInfo_ReturnsAllItems()
        {
            // Arrange
            int resultsCount = 3;

            // Act
            var okResult = await _controller.GetVehiclesInfo();

            // Assert
            var result = Assert.IsType<OkObjectResult>(okResult.Result);
            var vehicles = Assert.IsAssignableFrom<IEnumerable<Vehicle>>(result.Value);

            Assert.Equal(resultsCount, vehicles.Count());
        }

        [Fact]
        public async void Test_GetVehicleInfo_InvalidIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            int vehicleId = 1000;

            // Act
            var result = await _controller.GetVehicleInfo(vehicleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void Test_GetVehicleInfo_ValidIdPassed_ReturnsCorrectVehicle()
        {
            // Arrange
            int vehicleId = 2;
            string vehicleDescription = "Accidented common vehicle";

            // Act
            var result = await _controller.GetVehicleInfo(vehicleId);

            // Assert
            var foundResult = Assert.IsType<OkObjectResult>(result.Result);
            var vehicle = Assert.IsAssignableFrom<Vehicle>(foundResult.Value);

            Assert.Equal(vehicleDescription, vehicle.Description);
        }
    }
}