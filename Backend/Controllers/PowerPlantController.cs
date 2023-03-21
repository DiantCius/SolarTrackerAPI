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
        public async Task<ActionResult<PowerplantListResponse>> GetAllPowerplantsForUser(CancellationToken cancellationToken)
        {
            var powerplants = await _powerplantsHandler.GetAllPowerplantsForUserAsync(cancellationToken);
            return Ok(powerplants);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Powerplant>> GetPowerplantById(int id, CancellationToken cancellationToken)
        {
            var powerplant = await _powerplantsHandler.GetPowerPlantsByIdAsync(id, cancellationToken);
            return Ok(powerplant);
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PowerplantListResponse>> GetAllPowerplants(CancellationToken cancellationToken)
        {
            var powerplants = await _powerplantsHandler.GetAllPowerplantsForUserAsync(cancellationToken);
            return Ok(powerplants);
        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CreatePowerplantResponse>> CreatePowerplant([FromBody] CreatePowerplantRequest createPowerplantRequest, CancellationToken cancellationToken)
        {
            var response = await _powerplantsHandler.CreatePowerPlantAsync(createPowerplantRequest, cancellationToken);
            return Ok(response);
        }

        [HttpPost("status/update")]
        public async Task<ActionResult<ConnectionStatus>> UpdatePowerplantStatus([FromBody] UpdatePowerplantStatusRequest updatePowerplantStatusRequest, CancellationToken cancellationToken)
        {
            var response = await _powerplantsHandler.UpdatePowerplantStatusAsync(updatePowerplantStatusRequest, cancellationToken);
            return Ok(response);
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Powerplant>> DeletePowerplant(int id, CancellationToken cancellationToken)
        {
            var response = await _powerplantsHandler.DeletePowerplantAsync(id, cancellationToken);
            return Ok(response);
        }
    }
}
