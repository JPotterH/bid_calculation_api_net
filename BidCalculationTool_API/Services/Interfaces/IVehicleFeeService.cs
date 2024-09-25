using BidCalculationTool_API.Models;

namespace BidCalculationTool_API.Services.Interfaces
{
    public interface IVehicleFeeService
    {
        Task<VehicleFeeResult> GetFeesInfo(decimal bidOffer, int vehicleType);
    }
}
