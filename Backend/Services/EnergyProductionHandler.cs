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

        public EnergyProduction GetDailyProduction(int day, List<EnergyProduction> productions)
        {
            var response = productions.Where(x => x.CurrentTime.Day == day).OrderByDescending(x => x.DailyProduction).First();
            return response;
        }

        public async Task<long> GetTotalEnergyProductionFromYear(YearlyEnergyProductionRequest request, CancellationToken cancellationToken)
        {
            var production = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == request.serialNumber && x.CurrentTime.Year == request.year && x.CurrentTime.Year == request.year)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);
            var long_sum = production.Select(e => long.Parse(e.DailyProduction));
            var sum = long_sum.Sum();
            return sum;
        }

        public async Task<EnergyProductionsResponse> GetDailyEnergyProductionsFromMonthAsync(MonthlyProductionsRequest request, CancellationToken cancellationToken)
        {
            Powerplant powerPlant = await CheckPowerplant(request.serialNumber, cancellationToken);

            var monthlyProductions = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == request.serialNumber && x.CurrentTime.Month == request.month && x.CurrentTime.Year == request.year)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);

            if (!monthlyProductions.Any())
            {
                throw new ApiException($"No energy productions for this date", HttpStatusCode.NotFound);
            }

            var days = monthlyProductions
                .Select(x => x.CurrentTime.Day)
                .Distinct();

            List<EnergyProduction> list = new List<EnergyProduction>();

            foreach (int day in days)
            {
                var production = GetDailyProduction(day, monthlyProductions);
                list.Add(production);
            }

            return new EnergyProductionsResponse(list);
        }


    }
}
