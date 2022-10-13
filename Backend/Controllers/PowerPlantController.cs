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
        private readonly PowerplantsHandler _powerplantsHandler;

        public PowerplantController(PowerplantsHandler powerplantsHandler)
        {
            _powerplantsHandler = powerplantsHandler;
        }


        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PowerplantListResponse>> GetAllPowerPlantsForUser(CancellationToken cancellationToken)
        {
            var powerPlants = await _powerplantsHandler.GetAllPowerPlantsForUserAsync(cancellationToken);
            return Ok(powerPlants);
        }
        
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PowerplantListResponse>> GetAllPowerPlants(CancellationToken cancellationToken)
        {
            var powerPlants = await _powerplantsHandler.GetAllPowerPlantsForUserAsync(cancellationToken);
            return Ok(powerPlants);
        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CreatePowerplantResponse>> CreatePowerPlant([FromBody] CreatePowerplantRequest createPowerplantRequest, CancellationToken cancellationToken)
        {
            var response = await _powerplantsHandler.CreatePowerPlantAsync(createPowerplantRequest, cancellationToken);
            return Ok(response);
        }

        [HttpPost("status/update")]
        public async Task<ActionResult<ConnectionStatus>> UpdatePowerPlantStatus([FromBody] UpdatePowerplantStatusRequest updatePowerplantStatusRequest, CancellationToken cancellationToken)
        {
            var response = await _powerplantsHandler.UpdatePowerPlantStatusAsync(updatePowerplantStatusRequest, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<DeletePowerplantResponse>> DeletePowerPlant(DeletePowerplantRequest deletePowerplantRequest, CancellationToken cancellationToken)
        {
            var response = await _powerplantsHandler.DeletePowerPlantAsync(deletePowerplantRequest, cancellationToken);
            return Ok(response);
        }
    }
}
