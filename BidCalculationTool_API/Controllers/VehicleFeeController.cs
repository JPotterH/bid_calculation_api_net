using BidCalculationTool_API.Models;
using BidCalculationTool_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BidCalculationTool_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleFeeController : ControllerBase
    {
        private readonly IVehicleFeeService _vehicleFeeService;

        public VehicleFeeController(IVehicleFeeService vehicleFeeService)
        {
            _vehicleFeeService = vehicleFeeService;
        }


        [HttpGet]
        public async Task<ActionResult<VehicleFeeResult>> GetVehicleBidDetails(decimal bidOffer, int vehicleType)
        {
            try
            {
                var vehicleFeesInfo = await _vehicleFeeService.GetFeesInfo(bidOffer, vehicleType);
                return Ok(vehicleFeesInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
