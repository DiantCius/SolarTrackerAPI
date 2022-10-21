using Backend.DTO;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class IndicationsController : ControllerBase
    {
        private readonly IndicationService _indicationService;

        public IndicationsController(IndicationService indicationService)
        {
            _indicationService = indicationService;
        }

        [HttpPost("update")]
        public async Task UpdateOrAddIndication([FromBody] IndicationDto solarTrackerIndication, CancellationToken cancellationToken)
        {
            await _indicationService.UpdateOrAddIndication(solarTrackerIndication, cancellationToken);
        }

        [HttpGet("read")]
        public async Task<IndicationDto> GetIndication(string serialNumber, CancellationToken cancellationToken)
        {
            var response = await _indicationService.GetIndication(serialNumber, cancellationToken);
            return response;
        }
    }
}
