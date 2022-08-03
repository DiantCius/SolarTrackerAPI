using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class IndicationsController : ControllerBase
    {
        private readonly IndicationService _indicationService;

        public IndicationsController(IndicationService indicationService)
        {
            _indicationService = indicationService;
        }

        [HttpPost("update")]
        public async Task UpdateIndication([FromBody] SolarTrackerIndication solarTrackerIndication, CancellationToken cancellationToken)
        {
            await _indicationService.UpdateSolarTrackerIndication(solarTrackerIndication, cancellationToken);
        }

        [HttpGet("read")]
        public SolarTrackerIndication GetIndication(string serialNumber)
        {
            var response = _indicationService.GetSolarTrackerIndication(serialNumber);
            return response;
        }
    }
}
