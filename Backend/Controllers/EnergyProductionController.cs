using Backend.DataAccess;
using Backend.DTO.Requests;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class EnergyProductionController : ControllerBase
    {
        private readonly EnergyProductionHandler energyProductionHandler;

        public EnergyProductionController(EnergyProductionHandler energyProductionHandler)
        {
            this.energyProductionHandler = energyProductionHandler;
        }

        [HttpPost("add")]
        public async Task AddProduction([FromBody] EnergyProduction energyProduction, CancellationToken cancellationToken)
        {
            await energyProductionHandler.AddEnergyProduction(energyProduction, cancellationToken);
        }

        [HttpPost("addmultiple")]
        public async Task AddMultipleProductions([FromBody] AddEnergyProductionsRequest addEnergyProductionsRequest, CancellationToken cancellationToken)
        {
            await energyProductionHandler.AddEnergyProductions(addEnergyProductionsRequest, cancellationToken);
        }
        [HttpGet("all")]
        public async Task<List<EnergyProduction>> GetAllProductions(string serialNumber, CancellationToken cancellationToken)
        {
            var response =  await energyProductionHandler.GetAllEnergyProductions(serialNumber, cancellationToken);
            return response;
        }

        [HttpGet("today")]
        public async Task<List<EnergyProduction>> GetEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            var response = await energyProductionHandler.GetAllEnergyProductionsFromToday(serialNumber, cancellationToken);
            return response;
        }
    }
}
