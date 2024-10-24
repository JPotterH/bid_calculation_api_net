using BidCalculationTool_API.Controllers;
using BidCalculationTool_API.Models;
using BidCalculationTool_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace BidCalculationTool_UnitTests.UnitTests
{
    public class VehicleFeesUnitTest
    {
        private readonly Mock<IVehicleFeeService> _mockVehicleFeeService;
        private readonly VehicleFeeController _controller;

        public VehicleFeesUnitTest()
        {
            _mockVehicleFeeService = new Mock<IVehicleFeeService>();
            _controller = new VehicleFeeController(_mockVehicleFeeService.Object);
        }

        [Fact]
        public async void Test_GetVehicleBidDetails_ReturnsOkResult()
        {
            // Arrange
            decimal vehicleBasePrice = 300;
            int vehicleType = 2;

            // Act
            var result = await _controller.GetVehicleBidDetails(vehicleBasePrice, vehicleType);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result as OkObjectResult);
        }

        [Fact]
        public async void Test_GetVehicleBidDetails_ValidValuesPassed_ReturnsRightResult()
        {
            // Arrange
            decimal vehicleBasePrice = 500;
            int vehicleType = 1;
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

            _mockVehicleFeeService.Setup(service => service.GetFeesInfo(vehicleBasePrice, vehicleType)).ReturnsAsync(expectedFeeResult);

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
        public async void Test_GetVehicleBidDetails_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            decimal vehicleBasePrice = 1000;
            int vehicleType = 4;
            string exceptionMessage = "Invalid vehicle type.";

            _mockVehicleFeeService.Setup(service => service.GetFeesInfo(vehicleBasePrice, vehicleType))
                .ThrowsAsync(new System.Exception(exceptionMessage));

            // Act
            var result = await _controller.GetVehicleBidDetails(vehicleBasePrice, vehicleType);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }
    }
}