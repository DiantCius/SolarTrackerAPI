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
    public class PowerplantsHandler
    {
        private readonly ApplicationContext _context;
        private readonly CurrentUser _currentUser;

        public PowerplantsHandler(ApplicationContext context,  CurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
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
                PowerplantType = createPowerplantRequest.PowerplantType,
                ConnectionStatus = ConnectionStatus.Disconnected,
                SerialNumber = createPowerplantRequest.SerialNumber,
                Tariff= createPowerplantRequest.Tariff,
                City= createPowerplantRequest.City,
                User = user,
                UserId = user.UserId
            };
            await _context.Powerplants.AddAsync(newPowerplant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreatePowerplantResponse(newPowerplant.Name,newPowerplant.City, newPowerplant.Latitude, newPowerplant.Longitude, newPowerplant.Tariff, newPowerplant.PowerplantType, newPowerplant.SerialNumber, newPowerplant.ConnectionStatus);
        }

        public async Task<Powerplant> DeletePowerplantAsync(int id, CancellationToken cancellationToken)
        {
            var powerplantToDelete = await _context.Powerplants.FirstOrDefaultAsync(x => x.PowerplantId == id,cancellationToken);
            if (powerplantToDelete == null)
            {
                throw new ApiException($"Powerplant with: {id} id not found", HttpStatusCode.NotFound);
            }

            _context.Powerplants.Remove(powerplantToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            return powerplantToDelete;
        }

        public async Task<PowerplantListResponse> GetAllPowerplantsAsync(CancellationToken cancellationToken)
        {
            var powerplantList = await _context.Powerplants.OrderBy(x => x.PowerplantId).ToListAsync(cancellationToken);
            return new PowerplantListResponse(powerplantList);
        }

        public async Task<PowerplantListResponse> GetAllPowerplantsForUserAsync(CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstAsync(x => x.Username == _currentUser.GetCurrentUsername(), cancellationToken);

            var powerplantList = await _context.Powerplants.Where(x => x.UserId == user.UserId).OrderBy(x => x.PowerplantId).ToListAsync(cancellationToken);
            return new PowerplantListResponse(powerplantList);
        }

        public async Task<Powerplant> GetPowerPlantsByIdAsync(int id, CancellationToken cancellationToken)
        {
            var powerplant = await _context.Powerplants.FirstOrDefaultAsync(x => x.PowerplantId == id, cancellationToken);
            if (powerplant == null)
            {
                throw new ApiException($"Powerplant with id: {id} not found ", HttpStatusCode.NotFound);
            }
            return powerplant;
        }


        public async Task<ConnectionStatus> UpdatePowerplantStatusAsync(UpdatePowerplantStatusRequest updatePowerplantStatusRequest, CancellationToken cancellationToken)
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
