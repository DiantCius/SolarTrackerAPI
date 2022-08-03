using Backend.DataAccess;
using Backend.DTO.Requests;
using Backend.Errors;
using Backend.Models;
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

        public async Task AddEnergyProduction(EnergyProduction energyProduction, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(energyProduction.SerialNumber, cancellationToken);

            await _firebaseRepository.AddEnergyProduction(energyProduction, cancellationToken);
        }

        private async Task CheckPowerPlantStatus(string serialNumber, CancellationToken cancellationToken)
        {
            if (await _applicationContext.PowerPlants.Where(x => x.SerialNumber == serialNumber).AnyAsync(cancellationToken) == false)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} doesnt exist", HttpStatusCode.BadRequest);
            }

            if (_applicationContext.PowerPlants.Where(x => x.SerialNumber == serialNumber).FirstOrDefault().ConnectionStatus == ConnectionStatus.Disconnected)
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

        public async Task<List<EnergyProduction>> GetAllEnergyProductions(string serialNumber, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(serialNumber, cancellationToken);

            var response = await _firebaseRepository.GetAllEnergyProductions(serialNumber, cancellationToken);
            return response;
        }

        public async Task<List<EnergyProduction>> GetAllEnergyProductionsFromToday(string serialNumber, CancellationToken cancellationToken)
        {
            await CheckPowerPlantStatus(serialNumber, cancellationToken);

            var response = await _firebaseRepository.GetAllEnergyProductionsFromToday(serialNumber, cancellationToken);
            return response;
        }
    }
}
