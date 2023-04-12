using Backend.DataAccess;
using Backend.DTO;
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
    public class EnergyProductionController : ControllerBase
    {
        private readonly EnergyProductionHandler energyProductionHandler;

        public EnergyProductionController(EnergyProductionHandler energyProductionHandler)
        {
            this.energyProductionHandler = energyProductionHandler;
        }

        [HttpPost("add")]
        public async Task AddProduction([FromBody] EnergyProductionDto energyProduction, CancellationToken cancellationToken)
        {
            await energyProductionHandler.AddEnergyProduction(energyProduction, cancellationToken);
        }
        
        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<EnergyProductionsResponse> GetAllProductions(string serialNumber, CancellationToken cancellationToken)
        {
            var response = await energyProductionHandler.GetAllEnergyProductions(serialNumber, cancellationToken);
            return response;
        }

        [HttpGet("month/daily")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<EnergyProductionsResponse> GetDailyEnergyProductionsFromMonth(DailyProductionsFromMonthRequest request, CancellationToken cancellationToken)
        {
            var response = await energyProductionHandler.GetDailyEnergyProductionsFromMonthAsync(request, cancellationToken);
            return response;
        }

        [HttpGet("today")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<EnergyProductionsResponse> GetEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            var response = await energyProductionHandler.GetAllEnergyProductionsFromToday(serialNumber, cancellationToken);
            return response;
        }

        [HttpGet("today/last")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<EnergyProduction> GetLastEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            var response = await energyProductionHandler.GetLastEnergyProduction(serialNumber, cancellationToken);
            return response;
        }

        [HttpGet("day")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<EnergyProductionsResponse> GetEnergyProductionsFromDay(DailyProductionsRequest request, CancellationToken cancellationToken)
        {
            var response = await energyProductionHandler.GetAllEnergyProductionsFromDay(request, cancellationToken);
            return response;
        }


        [HttpGet("year/monthly")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<MonthlyEnergyProductionsResponse> GetTotalMonthlyEnergyProductionsFromYear(MonthlyProductionsFromYearRequest request, CancellationToken cancellationToken)
        {

            var response = await energyProductionHandler.GetMonthlyEnergyProductionsFromYearAsync(request, cancellationToken);
            return response;
        }

        [HttpGet("year/all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<YearlyEnergyProductionsResponse> GetTotalEnergyProductionFromYear(YearlyEnergyProductionRequest request, CancellationToken cancellationToken)
        {

            var response = await energyProductionHandler.GetYearlyEnergyProductions(request, cancellationToken);
            return response;
        }
    }
}
