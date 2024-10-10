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
            // Redundant data integrity validations to protect more the service and improve clarity on user feedback
            if (bidOffer <= 0m)
                throw new Exception("Vehicle's price must be greater than zero.");

            bool isValid = Enum.IsDefined(typeof(VehicleTypeEnum), vehicleType);
            if (!isValid)
                throw new Exception("Vehicle's type is not valid.");

            // Fixed and Variable Costs calculation
            const decimal FIXED_STORAGE_FEE = 100;

            // Each fee is calculated independently for more clarity and future scalability
            decimal basicBuyerFee = CalculateBasicBuyerFee(bidOffer, vehicleType);
            decimal sellerSpecialFee = CalculateSellerSpecialFee(bidOffer, vehicleType);
            decimal associationFee = CalculateAssociationFee(bidOffer);

            decimal totalFees = basicBuyerFee + sellerSpecialFee + associationFee + FIXED_STORAGE_FEE;
            decimal totalBidPlusFees = totalFees;

            // Vehicle fees DTO guaranties data integrity in communication with clients and can be easily updated
            var vehicleFees = new VehicleFeeResult() {
                BasicBuyerFee = basicBuyerFee,
                SellerSpecialFee = sellerSpecialFee,
                AssociationFee = associationFee,
                StorageFee = FIXED_STORAGE_FEE,
                TotalFee = totalFees,
                TotalBid = totalBidPlusFees,
            };

            return await Task.FromResult(vehicleFees);
        }

        /*
         * Constants are declared inside the code at this point and not defined in configuration
         * file, which would keep all values in a single place and making easier to update, but
         * as they have a very localized use, declaring them directly where they are going to be
         * used makes less probable to left unnecessary debris in case of modifications.
         */

        // Calculate Basic buyer fee using business rules for Common vehicles
        private static decimal CalculateBasicBuyerFeeCommon(decimal basicBuyerFee)
        {
            const decimal COMMON_BASIC_BUYER_FEE_MIN = 10m;
            const decimal COMMON_BASIC_BUYER_FEE_MAX = 50m;

            return Math.Clamp(basicBuyerFee, COMMON_BASIC_BUYER_FEE_MIN, COMMON_BASIC_BUYER_FEE_MAX);
        }

        // Calculate Basic buyer fee using business rules for Luxury vehicles
        private static decimal CalculateBasicBuyerFeeLuxury(decimal basicBuyerFee)
        {
            const decimal LUXURY_BASIC_BUYER_FEE_MIN = 25m;
            const decimal LUXURY_BASIC_BUYER_FEE_MAX = 200m;

            return Math.Clamp(basicBuyerFee, LUXURY_BASIC_BUYER_FEE_MIN, LUXURY_BASIC_BUYER_FEE_MAX);
        }

        // Calculate Basic buyer fee based on vehicle type
        // Implemented using Strategy pattern to improve clarity and scalability
        private static decimal CalculateBasicBuyerFee(decimal bidOffer, int vehicleType)
        {
            const decimal BASIC_BUYER_FEE_TAX = 0.1m;

            decimal basicBuyerFee = BASIC_BUYER_FEE_TAX * bidOffer;

            return vehicleType switch
            {
                (int)VehicleTypeEnum.Common => CalculateBasicBuyerFeeCommon(basicBuyerFee),
                (int)VehicleTypeEnum.Luxury => CalculateBasicBuyerFeeLuxury(basicBuyerFee),
                _ => -1,
            };
        }

        // Calculate the seller's special fee using business rules
        // Implemented using variant of Factory pattern, could be switched to Strategy pattern to improve
        // clarity and scalability if business logic changes or many vehicle types are added
        private static decimal CalculateSellerSpecialFee(decimal bidOffer, int vehicleType)
        {
            const decimal COMMON_SELLER_SPECIAL_FEE_TAX = 0.02m;
            const decimal LUXURY_SELLER_SPECIAL_FEE_TAX = 0.04m;

            return vehicleType switch
            {
                (int)VehicleTypeEnum.Common => COMMON_SELLER_SPECIAL_FEE_TAX * bidOffer,
                (int)VehicleTypeEnum.Luxury => LUXURY_SELLER_SPECIAL_FEE_TAX * bidOffer,
                _ => -1,
            };
        }

        // Calculate the added costs for the association based on the price of the vehicle
        private static decimal CalculateAssociationFee(decimal bidOffer)
        {
            if (bidOffer <= 500)
                return 5m;

            else if (bidOffer <= 1000)
                return 10m;

            else if (bidOffer <= 3000)
                return 15m;

            else
                return 20m;

        }
    }
}
