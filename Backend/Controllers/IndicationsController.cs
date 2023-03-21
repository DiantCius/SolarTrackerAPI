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

        [HttpPut("update")]
        public async Task UpdateIndication([FromBody] IndicationDto solarTrackerIndication, CancellationToken cancellationToken)
        {
            await _indicationService.UpdateIndicationAsync(solarTrackerIndication, cancellationToken);
        }

        [HttpPost("create")]
        public async Task CreateIndication([FromBody] IndicationDto solarTrackerIndication, CancellationToken cancellationToken)
        {
            await _indicationService.AddIndicationAsync(solarTrackerIndication, cancellationToken);
        }

        [HttpGet("read")]
        public async Task<IndicationDto> GetIndication(string serialNumber, CancellationToken cancellationToken)
        {
            var response = await _indicationService.GetIndication(serialNumber, cancellationToken);
            return response;
        }
    }
}
