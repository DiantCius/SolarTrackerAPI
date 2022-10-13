using Backend.DataAccess;
using Backend.DTO;
using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Errors;
using Backend.Models;
using Google.Api;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class EnergyProductionHandler
    {
        private readonly FirebaseRepository _firebaseRepository;
        private readonly ApplicationContext _applicationContext;

        public EnergyProductionHandler(FirebaseRepository firebaseRepository, ApplicationContext applicationContext)
        {
            _firebaseRepository = firebaseRepository;
            _applicationContext = applicationContext;
        }

        public async Task AddEnergyProduction(EnergyProductionDto energyProduction, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(energyProduction.SerialNumber, cancellationToken);

            var newEnergyProduction = new EnergyProduction()
            {
                CurrentProduction = energyProduction.CurrentProduction,
                DailyProduction = energyProduction.DailyProduction,
                CurrentTime = energyProduction.CurrentTime,
                SerialNumber = energyProduction.SerialNumber,
            };

            await _applicationContext.EnergyProductions.AddAsync(newEnergyProduction, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);


            // add energy production to firebase
            var firebaseEnergyProduction = new FirebaseEnergyProduction()
            {
                CurrentProduction = energyProduction.CurrentProduction,
                DailyProduction = energyProduction.DailyProduction,
                CurrentTime = energyProduction.CurrentTime,
                SerialNumber = energyProduction.SerialNumber,
            };

            await _firebaseRepository.AddEnergyProduction(firebaseEnergyProduction, cancellationToken);
        }

        private async Task CheckPowerPlantStatus(string serialNumber, CancellationToken cancellationToken)
        {
            if (await _applicationContext.Powerplants.Where(x => x.SerialNumber == serialNumber).AnyAsync(cancellationToken) == false)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} doesnt exist", HttpStatusCode.BadRequest);
            }

            if (_applicationContext.Powerplants.Where(x => x.SerialNumber == serialNumber).FirstOrDefault().ConnectionStatus == ConnectionStatus.Disconnected)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} isnt connected", HttpStatusCode.BadRequest);
            }
        }

        public async Task AddEnergyProductions(AddEnergyProductionsRequest addEnergyProductionsRequest, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(addEnergyProductionsRequest.SerialNumber, cancellationToken);

            await _firebaseRepository.AddEnergyProductions(addEnergyProductionsRequest, cancellationToken);
        }

        public async Task DeleteCollection(string serialNumber, int batchSize, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(serialNumber, cancellationToken);

            await _firebaseRepository.DeleteCollection(serialNumber, batchSize);
        }

        public async Task<EnergyProductionsResponse> GetAllEnergyProductions(string serialNumber, CancellationToken cancellationToken)
        {
            var energyProductions = await _applicationContext.EnergyProductions.Where(x => x.SerialNumber == serialNumber).OrderBy(x => x.CurrentTime).ToListAsync(cancellationToken);
            return new EnergyProductionsResponse(energyProductions);
        }

        /*public async Task<List<FirebaseEnergyProduction>> GetAllEnergyProductions(string serialNumber, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(serialNumber, cancellationToken);

            var response = await _firebaseRepository.GetAllEnergyProductions(serialNumber, cancellationToken);
            return response;
        }*/

        public async Task<EnergyProductionsResponse> GetAllEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            var energyProductions = await _applicationContext.EnergyProductions
                .Where(x => x.SerialNumber == serialNumber && x.CurrentTime >= DateTime.Today)
                .OrderBy(x => x.CurrentTime)
                .ToListAsync(cancellationToken);

            return new EnergyProductionsResponse(energyProductions);
        }

        /*public async Task<List<FirebaseEnergyProduction>> GetAllEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(serialNumber, cancellationToken);

            var response = await _firebaseRepository.GetAllEnergyProductionsFromToday(serialNumber, cancellationToken);
            return response;
        }*/
    }
}
