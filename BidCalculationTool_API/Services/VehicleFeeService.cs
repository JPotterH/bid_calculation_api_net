using BidCalculationTool_API.Models;
using BidCalculationTool_API.Repositories.Interfaces;
using BidCalculationTool_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BidCalculationTool_API.Services
{
    public class VehicleFeeService : IVehicleFeeService
    {
        protected readonly IVehicleRepository _vehicleRepository;

        public VehicleFeeService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<VehicleFeeResult> GetFeesInfo(decimal bidOffer, int vehicleType)
        {
            if (bidOffer <= 0m)
                throw new Exception("Vehicle's price must be greater than zero.");

            bool isValid = Enum.IsDefined(typeof(VehicleTypeEnum), vehicleType);
            if (!isValid)
                throw new Exception("Vehicle's type is not valid.");

            const decimal FIXED_STORAGE_FEE = 100;
            decimal basicBuyerFee = CalculateBasicBuyerFee(bidOffer, vehicleType);
            decimal sellerSpecialFee = CalculateSellerSpecialFee(bidOffer, vehicleType);
            decimal associationFee = CalculateAssociationFee(bidOffer);
            decimal totalFees = basicBuyerFee + sellerSpecialFee + associationFee + FIXED_STORAGE_FEE;

            var vehicleFees = new VehicleFeeResult() {
                BasicBuyerFee = basicBuyerFee,
                SellerSpecialFee = sellerSpecialFee,
                AssociationFee = associationFee,
                StorageFee = FIXED_STORAGE_FEE,
                TotalFee = totalFees
            };

            return await Task.FromResult(vehicleFees);
        }

        private static decimal CalculateBasicBuyerFee(decimal bidOffer, int vehicleType)
        {
            const decimal BASIC_BUYER_FEE_TAX = 0.1m;
            const decimal BASIC_BUYER_FEE_COMMON_MIN = 10m;
            const decimal BASIC_BUYER_FEE_COMMON_MAX = 50m;
            const decimal BASIC_BUYER_FEE_LUXURY_MIN = 25m;
            const decimal BASIC_BUYER_FEE_LUXURY_MAX = 200m;

            decimal basicBuyerFee = BASIC_BUYER_FEE_TAX * bidOffer;

            return vehicleType switch
            {
                (int)VehicleTypeEnum.Common => Math.Clamp(basicBuyerFee, BASIC_BUYER_FEE_COMMON_MIN, BASIC_BUYER_FEE_COMMON_MAX),
                (int)VehicleTypeEnum.Luxury => Math.Clamp(basicBuyerFee, BASIC_BUYER_FEE_LUXURY_MIN, BASIC_BUYER_FEE_LUXURY_MAX),
                _ => -1,
            };
        }

        private static decimal CalculateSellerSpecialFee(decimal bidOffer, int vehicleType)
        {
            const decimal SELLER_SPECIAL_FEE_COMMON_TAX = 0.02m;
            const decimal SELLER_SPECIAL_FEE_LUXURY_TAX = 0.04m;

            return vehicleType switch
            {
                (int)VehicleTypeEnum.Common => SELLER_SPECIAL_FEE_COMMON_TAX * bidOffer,
                (int)VehicleTypeEnum.Luxury => SELLER_SPECIAL_FEE_LUXURY_TAX * bidOffer,
                _ => -1,
            };
        }

        private static decimal CalculateAssociationFee(decimal bidOffer)
        {
            if (bidOffer <= 500)
            {
                return 5m;
            }
            else if (bidOffer <= 1000)
            {
                return 10m;
            }
            else if (bidOffer <= 3000)
            {
                return 15m;
            }
            else
            {
                return 20m;
            }
        }
    }
}
