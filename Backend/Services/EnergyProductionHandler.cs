using Backend.DataAccess;
using Backend.DTO;
using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Errors;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Globalization;
using System.Net;
using System.Threading;

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

        public async Task<EnergyProduction> GetLastEnergyProduction(string serialNumber, CancellationToken cancellationToken)
        {
            var energyProduction = await _applicationContext.EnergyProductions.Where(x=> x.CurrentTime.Day == DateTime.Today.Day).OrderByDescending(x => x.EnergyProductionId).FirstOrDefaultAsync(cancellationToken);
            if (energyProduction == null)
            {
                throw new ApiException($"Production not found", HttpStatusCode.NotFound);
            }

            return energyProduction;
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


        public async Task<EnergyProductionsResponse> GetAllEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            Powerplant powerPlant = await CheckPowerplant(serialNumber, cancellationToken);

            var energyProductions = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == serialNumber && x.CurrentTime >= DateTime.Today)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);

            return new EnergyProductionsResponse(energyProductions);
        }

        public async Task<EnergyProductionsResponse> GetAllEnergyProductionsFromDay(DailyProductionsRequest request, CancellationToken cancellationToken)
        {
            Powerplant powerPlant = await CheckPowerplant(request.serialNumber, cancellationToken);

            var energyProductions = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == request.serialNumber && x.CurrentTime.Year == request.year && x.CurrentTime.Month == request.month && x.CurrentTime.Day==request.day)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);

            return new EnergyProductionsResponse(energyProductions);
        }

        public async Task<EnergyProductionsResponse> GetAllEnergyProductions(string serialNumber, CancellationToken cancellationToken)
        {
            var energyProductions = await _applicationContext.EnergyProductions.Where(x => x.SerialNumber == serialNumber).OrderBy(x => x.CurrentTime).ToListAsync(cancellationToken);
            return new EnergyProductionsResponse(energyProductions);
        }

        public EnergyProduction GetDailyProduction(int day, List<EnergyProduction> productions)
        {
            var response = productions.Where(x => x.CurrentTime.Day == day).OrderByDescending(x => x.EnergyProductionId).First();
            return response;
        }

        public float GetYearlyProduction(int year, List<EnergyProduction> productions)
        {
            var yearlyProductions = productions.Where(x => x.CurrentTime.Year == year).ToList();

            var months = yearlyProductions.Select(x => x.CurrentTime.Month).Distinct();

            float sum = 0;

            foreach (int month in months)
            {
                var productionAmount = GetMonthlyProduction(month, yearlyProductions);
                sum+= productionAmount;
            }
            return sum;
        }

        public async Task<YearlyEnergyProductionsResponse> GetYearlyEnergyProductions(YearlyEnergyProductionRequest request, CancellationToken cancellationToken)
        {
            var productions = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == request.serialNumber)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);
            if (!productions.Any())
            {
                throw new ApiException($"No energy productions for this year", HttpStatusCode.NotFound);
            }

            var years = productions.Select(x => x.CurrentTime.Year).Distinct();

            List<YearlyProduction> list = new List<YearlyProduction>();

            foreach (int year in years)
            {
                var productionAmount = GetYearlyProduction(year, productions);
                list.Add(new YearlyProduction(year, productionAmount));
            }

            return new YearlyEnergyProductionsResponse(request.serialNumber, list);
        }

        public float GetMonthlyProduction(int month, List<EnergyProduction> productions)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            float sum = 0;
            var monthlyProductions = productions.Where(x => x.CurrentTime.Month == month);
            var days = monthlyProductions.Select(x => x.CurrentTime.Day).Distinct();
            foreach(int day in days)
            {
                var production = monthlyProductions.Where(x => x.CurrentTime.Day == day).OrderByDescending(x => x.EnergyProductionId).First();
                sum = sum+ float.Parse(production.DailyProduction, nfi);
            }
            return sum;
        }


        public async Task<MonthlyEnergyProductionsResponse> GetMonthlyEnergyProductionsFromYearAsync(MonthlyProductionsFromYearRequest request, CancellationToken cancellationToken)
        {
            var yearlyProductions = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == request.serialNumber && x.CurrentTime.Year == request.year)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);

            if (!yearlyProductions.Any())
            {
                throw new ApiException($"No energy productions for this year", HttpStatusCode.NotFound);
            }

            var months = yearlyProductions.Select(x => x.CurrentTime.Month).Distinct();

            List<MonthlyProduction> list = new List<MonthlyProduction>();

            foreach (int month in months)
            {
                var productionAmount = GetMonthlyProduction(month, yearlyProductions);
                list.Add(new MonthlyProduction(month, request.year, productionAmount));
            }

            return new MonthlyEnergyProductionsResponse(request.serialNumber, list);
        }

        public async Task<EnergyProductionsResponse> GetDailyEnergyProductionsFromMonthAsync(DailyProductionsFromMonthRequest request, CancellationToken cancellationToken)
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
