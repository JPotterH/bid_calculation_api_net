using BidCalculationTool_API.Context;
using BidCalculationTool_API.Controllers;
using BidCalculationTool_API.Models;
using BidCalculationTool_API.Services;
using BidCalculationTool_API.Services.Interfaces;
using BidCalculationTool_UnitTests.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace BidCalculationTool_UnitTests.UnitTests
{
    public class VehicleFeesUnitTest
    {
        private readonly Mock<IVehicleFeeService> _stubVehicleFeeService;
        private readonly VehicleFeeController _stubController;

        private readonly InMemoryContext _context;
        private readonly VehicleRepository _vehicleRepository;
        private readonly IVehicleFeeService _mockVehicleFeeService;
        private readonly VehicleFeeController _controller;

        public VehicleFeesUnitTest()
        {
            _stubVehicleFeeService = new Mock<IVehicleFeeService>();
            _stubController = new VehicleFeeController(_stubVehicleFeeService.Object);

            var options = new DbContextOptionsBuilder<InMemoryContext>()
                .UseInMemoryDatabase("VehicleBidding")
                .Options;
            _context = new InMemoryContext(options);

            _vehicleRepository = new VehicleRepository(_context);
            _mockVehicleFeeService = new VehicleFeeService(_vehicleRepository);
            _controller = new VehicleFeeController(_mockVehicleFeeService);
        }

        // Warming up
        [Fact]
        public async void Test_GetVehicleBidDetails_ReturnsOkResult()
        {
            // Arrange
            decimal vehicleBasePrice = 300;
            int vehicleType = (int)VehicleTypeEnum.Common;

            // Act
            var result = await _stubController.GetVehicleBidDetails(vehicleBasePrice, vehicleType);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result as OkObjectResult);
        }

        [Fact]
        public async void Test_GetVehicleBidDetails_ValidValuesPassed_TypeCommon_ReturnsRightResult()
        {
            // Arrange
            decimal vehicleBasePrice = 500;
            int vehicleType = (int)VehicleTypeEnum.Common;
            var expectedFeeResult = new VehicleFeeResult
            {
                Bid = vehicleBasePrice,
                BasicBuyerFee = 50,
                SellerSpecialFee = 10,
                AssociationFee = 5,
                StorageFee = 100,
                TotalFee = 165,
                TotalBid = 665
            };

            // Act
            var result = await _controller.GetVehicleBidDetails(vehicleBasePrice, vehicleType);

            // Assert
            var foundResult = Assert.IsType<OkObjectResult>(result.Result);
            var vehicleFees = Assert.IsAssignableFrom<VehicleFeeResult>(foundResult.Value);

            Assert.Equal(expectedFeeResult.Bid, vehicleFees.Bid);
            Assert.Equal(expectedFeeResult.BasicBuyerFee, vehicleFees.BasicBuyerFee);
            Assert.Equal(expectedFeeResult.SellerSpecialFee, vehicleFees.SellerSpecialFee);
            Assert.Equal(expectedFeeResult.AssociationFee, vehicleFees.AssociationFee);
            Assert.Equal(expectedFeeResult.StorageFee, vehicleFees.StorageFee);
            Assert.Equal(expectedFeeResult.TotalFee, vehicleFees.TotalFee);
            Assert.Equal(expectedFeeResult.TotalBid, vehicleFees.TotalBid);
        }

        [Fact]
        public async void Test_GetVehicleBidDetails_ValidValuesPassed_TypeLuxury_ReturnsRightResult()
        {
            // Arrange
            decimal vehicleBasePrice = 1800;
            int vehicleType = (int)VehicleTypeEnum.Luxury;
            var expectedFeeResult = new VehicleFeeResult
            {
                Bid = vehicleBasePrice,
                BasicBuyerFee = 180,
                SellerSpecialFee = 72,
                AssociationFee = 15,
                StorageFee = 100,
                TotalFee = 367,
                TotalBid = 2167
            };

            // Act
            var result = await _controller.GetVehicleBidDetails(vehicleBasePrice, vehicleType);

            // Assert
            var foundResult = Assert.IsType<OkObjectResult>(result.Result);
            var vehicleFees = Assert.IsAssignableFrom<VehicleFeeResult>(foundResult.Value);

            Assert.Equal(expectedFeeResult.Bid, vehicleFees.Bid);
            Assert.Equal(expectedFeeResult.BasicBuyerFee, vehicleFees.BasicBuyerFee);
            Assert.Equal(expectedFeeResult.SellerSpecialFee, vehicleFees.SellerSpecialFee);
            Assert.Equal(expectedFeeResult.AssociationFee, vehicleFees.AssociationFee);
            Assert.Equal(expectedFeeResult.StorageFee, vehicleFees.StorageFee);
            Assert.Equal(expectedFeeResult.TotalFee, vehicleFees.TotalFee);
            Assert.Equal(expectedFeeResult.TotalBid, vehicleFees.TotalBid);
        }

        [Fact]
        public async void Test_GetVehicleBidDetails_ValidValuesPassed_TypeLuxury_ReturnsWrongResult()
        {
            // Arrange
            decimal vehicleBasePrice = 660;
            int vehicleType = (int)VehicleTypeEnum.Luxury;
            var expectedFeeResult = new VehicleFeeResult
            {
                Bid = vehicleBasePrice,
                BasicBuyerFee = 50,
                SellerSpecialFee = 10,
                AssociationFee = 10,
                StorageFee = 100,
                TotalFee = 165,
                TotalBid = 665
            };

            // Act
            var result = await _controller.GetVehicleBidDetails(vehicleBasePrice, vehicleType);

            // Assert
            var foundResult = Assert.IsType<OkObjectResult>(result.Result);
            var vehicleFees = Assert.IsAssignableFrom<VehicleFeeResult>(foundResult.Value);

            Assert.Equal(expectedFeeResult.Bid, vehicleFees.Bid);
            Assert.NotEqual(expectedFeeResult.BasicBuyerFee, vehicleFees.BasicBuyerFee);
            Assert.NotEqual(expectedFeeResult.SellerSpecialFee, vehicleFees.SellerSpecialFee);
            Assert.Equal(expectedFeeResult.AssociationFee, vehicleFees.AssociationFee);
            Assert.Equal(expectedFeeResult.StorageFee, vehicleFees.StorageFee);
            Assert.NotEqual(expectedFeeResult.TotalFee, vehicleFees.TotalFee);
            Assert.NotEqual(expectedFeeResult.TotalBid, vehicleFees.TotalBid);
        }

        [Fact]
        public async void Test_GetVehicleBidDetails_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            decimal vehicleBasePrice = 1000;
            int vehicleType = 4;
            string exceptionMessage = "Invalid vehicle type.";

            _stubVehicleFeeService.Setup(service => service.GetFeesInfo(vehicleBasePrice, vehicleType))
                .ThrowsAsync(new System.Exception(exceptionMessage));

            // Act
            var result = await _stubController.GetVehicleBidDetails(vehicleBasePrice, vehicleType);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }
    }
}