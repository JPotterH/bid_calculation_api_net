using BidCalculationTool_API.Models;
using BidCalculationTool_API.Services;
using BidCalculationTool_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BidCalculationTool_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehiclesInfo()
        {
            try
            {
                var vehicles = await _vehicleService.GetAll();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicleInfo(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetById(id);
                if (vehicle is null)
                    return NotFound($"Vehicle with Id {id} was not found.");

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
