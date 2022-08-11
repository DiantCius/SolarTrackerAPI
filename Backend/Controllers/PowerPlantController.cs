using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class PowerplantController : ControllerBase
    {
        private readonly PowerPlantsHandler _powerPlantsHandler;

        public PowerplantController(PowerPlantsHandler powerPlantsHandler)
        {
            _powerPlantsHandler = powerPlantsHandler;
        }


        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PowerPlantListResponse>> GetAllPowerPlantsForUser(CancellationToken cancellationToken)
        {
            var powerPlants = await _powerPlantsHandler.GetAllPowerPlantsForUserAsync(cancellationToken);
            return Ok(powerPlants);
        }
        
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PowerPlantListResponse>> GetAllPowerPlants(CancellationToken cancellationToken)
        {
            var powerPlants = await _powerPlantsHandler.GetAllPowerPlantsForUserAsync(cancellationToken);
            return Ok(powerPlants);
        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CreatePowerPlantResponse>> CreatePowerPlant([FromBody] CreatePowerPlantRequest createPowerPlantRequest, CancellationToken cancellationToken)
        {
            var response = await _powerPlantsHandler.CreatePowerPlantAsync(createPowerPlantRequest, cancellationToken);
            return Ok(response);
        }

        [HttpPost("status/update")]
        public async Task<ActionResult<ConnectionStatus>> UpdatePowerPlantStatus([FromBody] UpdatePowerPlantStatusRequest updatePowerPlantStatusRequest, CancellationToken cancellationToken)
        {
            var response = await _powerPlantsHandler.UpdatePowerPlantStatusAsync(updatePowerPlantStatusRequest, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeletePowerPlant(DeletePowerPlantRequest deletePowerPlantRequest, CancellationToken cancellationToken)
        {
            await _powerPlantsHandler.DeletePowerPlantAsync(deletePowerPlantRequest, cancellationToken);
            return Ok();
        }
    }
}
