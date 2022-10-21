using Backend.DataAccess;
using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Errors;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class PowerplantsHandler
    {
        private readonly ApplicationContext _context;
        private readonly CurrentUser _currentUser;
        private readonly FirebaseRepository _firebaseRepository;

        public PowerplantsHandler(ApplicationContext context,  CurrentUser currentUser, FirebaseRepository firebaseRepository)
        {
            _context = context;
            _currentUser = currentUser;
            _firebaseRepository = firebaseRepository;
        }

        public async Task<CreatePowerplantResponse> CreatePowerPlantAsync(CreatePowerplantRequest createPowerplantRequest, CancellationToken cancellationToken)
        {
            if (await _context.Powerplants.Where(x => x.SerialNumber == createPowerplantRequest.SerialNumber).AnyAsync(cancellationToken))
            {
                throw new ApiException($"Powerplant with serial number: {createPowerplantRequest.SerialNumber} already exists", HttpStatusCode.BadRequest);
            }

            var user = await _context.Users.FirstAsync(x => x.Username == _currentUser.GetCurrentUsername(), cancellationToken);

            var newPowerplant = new Powerplant()
            {
                Name = createPowerplantRequest.Name,
                Location = createPowerplantRequest.Location,
                PowerplantType = createPowerplantRequest.PowerplantType,
                ConnectionStatus = ConnectionStatus.Disconnected,
                SerialNumber = createPowerplantRequest.SerialNumber,
                User = user,
                UserId = user.UserId
            };

            await _context.Powerplants.AddAsync(newPowerplant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            //int[] state =  { 0, 0, 0 };
            //Indications.AddSolarTrackerIndication(new SolarTrackerIndication { SerialNumber = newPowerplant.SerialNumber, Azimuth = 0F, Elevation = 0F, WindSpeed = 0F, State = state });

            return new CreatePowerplantResponse(newPowerplant.Name,newPowerplant.Location, newPowerplant.PowerplantType, newPowerplant.SerialNumber, newPowerplant.ConnectionStatus);
        }

        public async Task<DeletePowerplantResponse> DeletePowerPlantAsync(DeletePowerplantRequest deletePowerPlantRequest, CancellationToken cancellationToken)
        {
            var powerplantToDelete = await _context.Powerplants.Where(x => x.SerialNumber == deletePowerPlantRequest.SerialNumber).FirstAsync(cancellationToken);
            var serialNumber = powerplantToDelete.SerialNumber;
            if (powerplantToDelete == null)
            {
                throw new ApiException($"Powerplant with: {deletePowerPlantRequest.SerialNumber} serial number not found", HttpStatusCode.NotFound);
            }

            _context.Powerplants.Remove(powerplantToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            await _firebaseRepository.DeleteCollection(serialNumber, 10);
            return new DeletePowerplantResponse(powerplantToDelete.Name, powerplantToDelete.Location, powerplantToDelete.PowerplantType, powerplantToDelete.SerialNumber, powerplantToDelete.ConnectionStatus);
        }

        public async Task<PowerplantListResponse> GetAllPowerPlantsAsync(CancellationToken cancellationToken)
        {
            var powerplantList = await _context.Powerplants.OrderBy(x => x.PowerplantId).ToListAsync(cancellationToken);
            return new PowerplantListResponse(powerplantList);
        }

        public async Task<PowerplantListResponse> GetAllPowerPlantsForUserAsync(CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstAsync(x => x.Username == _currentUser.GetCurrentUsername(), cancellationToken);

            var powerplantList = await _context.Powerplants.Where(x => x.UserId == user.UserId).OrderBy(x => x.PowerplantId).ToListAsync(cancellationToken);
            return new PowerplantListResponse(powerplantList);
        }

        public async Task<ConnectionStatus> UpdatePowerPlantStatusAsync(UpdatePowerplantStatusRequest updatePowerplantStatusRequest, CancellationToken cancellationToken)
        {
            var powerplantToUpdate = await _context.Powerplants.FirstAsync(x => x.SerialNumber == updatePowerplantStatusRequest.SerialNumber, cancellationToken);

            if (powerplantToUpdate == null)
            {
                throw new ApiException($"Powerplant with serial number: {updatePowerplantStatusRequest.SerialNumber} not found ", HttpStatusCode.NotFound);
            }

            powerplantToUpdate.ConnectionStatus = updatePowerplantStatusRequest.ConnectionStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return powerplantToUpdate.ConnectionStatus;
        }
            
    }
}
