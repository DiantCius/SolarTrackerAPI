using Backend.DataAccess;
using Backend.DTO;
using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Errors;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class EnergyProductionHandler
    {
        private readonly ApplicationContext _applicationContext;

        public EnergyProductionHandler(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task AddEnergyProduction(EnergyProductionDto energyProduction, CancellationToken cancellationToken)
        {
            Powerplant powerPlant = await CheckPowerplant(energyProduction.SerialNumber, cancellationToken);

            var newEnergyProduction = new EnergyProduction()
            {
                CurrentProduction = energyProduction.CurrentProduction,
                DailyProduction = energyProduction.DailyProduction,
                CurrentTime = DateTime.UtcNow,
                SerialNumber = energyProduction.SerialNumber,
                Powerplant = powerPlant,
            };

            await _applicationContext.EnergyProductions.AddAsync(newEnergyProduction, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);

        }

        private async Task<Powerplant> CheckPowerplant(String serialNumber, CancellationToken cancellationToken)
        {
            var powerPlant = await _applicationContext.Powerplants.FirstAsync(x => x.SerialNumber == serialNumber, cancellationToken);

            if (powerPlant == null)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} not found ", HttpStatusCode.NotFound);
            }

            if (powerPlant.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} is not connected ", HttpStatusCode.BadRequest);
            }

            return powerPlant;
        }


        public async Task<EnergyProductionsResponse> GetAllEnergyProductions(string serialNumber, CancellationToken cancellationToken)
        {
            var energyProductions = await _applicationContext.EnergyProductions.Where(x => x.SerialNumber == serialNumber).OrderBy(x => x.CurrentTime).ToListAsync(cancellationToken);
            return new EnergyProductionsResponse(energyProductions);
        }


        public async Task<EnergyProductionsResponse> GetAllEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            Powerplant powerPlant = await CheckPowerplant(serialNumber, cancellationToken);

            var energyProductions = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == serialNumber && x.CurrentTime >= DateTime.Today)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);

            return new EnergyProductionsResponse(energyProductions);
        }

    }
}
