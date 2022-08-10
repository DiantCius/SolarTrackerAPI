using Backend.DataAccess;
using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Errors;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class PowerPlantsHandler
    {
        private readonly ApplicationContext _context;
        private readonly CurrentUser _currentUser;
        private readonly IndicationService _indicationService;
        private readonly FirebaseRepository _firebaseRepository;

        public PowerPlantsHandler(ApplicationContext context,  CurrentUser currentUser, IndicationService indicationService, FirebaseRepository firebaseRepository)
        {
            _context = context;
            _currentUser = currentUser;
            _indicationService = indicationService;
            _firebaseRepository = firebaseRepository;
        }

        public async Task<CreatePowerPlantResponse> CreatePowerPlantAsync(CreatePowerPlantRequest createPowerPlantRequest, CancellationToken cancellationToken)
        {
            if (await _context.PowerPlants.Where(x => x.SerialNumber == createPowerPlantRequest.SerialNumber).AnyAsync(cancellationToken))
            {
                throw new ApiException($"Powerplant with serial number: {createPowerPlantRequest.SerialNumber} already exists", HttpStatusCode.BadRequest);
            }

            var user = await _context.Users.FirstAsync(x => x.Username == _currentUser.GetCurrentUsername(), cancellationToken);

            var newPowerPlant = new PowerPlant()
            {
                Name = createPowerPlantRequest.Name,
                Location = createPowerPlantRequest.Location,
                PowerPlantType = createPowerPlantRequest.PowerPlantType,
                ConnectionStatus = ConnectionStatus.Disconnected,
                SerialNumber = createPowerPlantRequest.SerialNumber,
                User = user,
                UserId = user.UserId
            };

            await _context.PowerPlants.AddAsync(newPowerPlant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            int[] state =  { 0, 0, 0 };
            Indications.AddSolarTrackerIndication(new SolarTrackerIndication { SerialNumber = newPowerPlant.SerialNumber, Azimuth = 0F, Elevation = 0F, WindSpeed = 0F, State = state });

            return new CreatePowerPlantResponse(newPowerPlant.Name,newPowerPlant.Location, newPowerPlant.PowerPlantType, newPowerPlant.SerialNumber, newPowerPlant.ConnectionStatus);
        }

        public async Task DeletePowerPlantAsync(DeletePowerPlantRequest deletePowerPlantRequest, CancellationToken cancellationToken)
        {
            var powerPlantToDelete = await _context.PowerPlants.Where(x => x.PowerPlantId == deletePowerPlantRequest.PowerPlantId).FirstAsync(cancellationToken);
            var serialNumber = powerPlantToDelete.SerialNumber;
            if (powerPlantToDelete == null)
            {
                throw new ApiException($"Powerplant with: {deletePowerPlantRequest.PowerPlantId} id not found", HttpStatusCode.NotFound);
            }

            _context.PowerPlants.Remove(powerPlantToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            await _firebaseRepository.DeleteCollection(serialNumber, 10);
        }

        public async Task<PowerPlantListResponse> GetAllPowerPlantsAsync(CancellationToken cancellationToken)
        {
            var powerPlantList = await _context.PowerPlants.OrderBy(x => x.PowerPlantId).ToListAsync(cancellationToken);
            return new PowerPlantListResponse(powerPlantList);
        }

        public async Task<PowerPlantListResponse> GetAllPowerPlantsForUserAsync(CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstAsync(x => x.Username == _currentUser.GetCurrentUsername(), cancellationToken);

            var powerPlantList = await _context.PowerPlants.Where(x => x.UserId == user.UserId).OrderBy(x => x.PowerPlantId).ToListAsync(cancellationToken);
            return new PowerPlantListResponse(powerPlantList);
        }

        public async Task<ConnectionStatus> UpdatePowerPlantStatusAsync(UpdatePowerPlantStatusRequest updatePowerPlantStatusRequest, CancellationToken cancellationToken)
        {
            var powerPlantToUpdate = await _context.PowerPlants.FirstAsync(x => x.SerialNumber == updatePowerPlantStatusRequest.SerialNumber, cancellationToken);

            if (powerPlantToUpdate == null)
            {
                throw new ApiException($"Powerplant with serial number: {updatePowerPlantStatusRequest.SerialNumber} not found ", HttpStatusCode.NotFound);
            }

            powerPlantToUpdate.ConnectionStatus = updatePowerPlantStatusRequest.ConnectionStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return powerPlantToUpdate.ConnectionStatus;
        }
            
    }
}
