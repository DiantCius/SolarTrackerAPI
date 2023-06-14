using Backend.DataAccess;
using Backend.DTO;
using Backend.DTO.Requests;
using Backend.Errors;
using Backend.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;

namespace Backend.Services
{
    public class IndicationService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IHubContext<IndicationsHub> _indicationsHub;

        public IndicationService(ApplicationContext applicationContext, IHubContext<IndicationsHub> indicationsHub)
        {
            _applicationContext = applicationContext;
            _indicationsHub = indicationsHub;
        }

        public async Task<IndicationDto> UpdateIndicationAsync(IndicationDto indicationDto, CancellationToken cancellationToken)
        {
            var powerPlantToUpdate = await _applicationContext.Powerplants.FirstOrDefaultAsync(x => x.SerialNumber == indicationDto.SerialNumber, cancellationToken);

            if (powerPlantToUpdate == null)
            {
                throw new ApiException($"Powerplant with serial number: {indicationDto.SerialNumber} not found ", HttpStatusCode.NotFound);
            }

            if (powerPlantToUpdate.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                throw new ApiException($"Powerplant with serial number: {indicationDto.SerialNumber} is not connected ", HttpStatusCode.BadRequest);
            }

            var powerplantIndication = new Indication()
            {
                SerialNumber = indicationDto.SerialNumber,
                Azimuth = indicationDto.Azimuth,
                Elevation = indicationDto.Elevation,
                WindSpeed = indicationDto.WindSpeed,
                State = indicationDto.State,
                Powerplant = powerPlantToUpdate,
                PowerplantId = powerPlantToUpdate.PowerplantId,
            };

            if (await _applicationContext.Indications.Where(x => x.SerialNumber == indicationDto.SerialNumber).AnyAsync(cancellationToken))
            {
                var indication = await _applicationContext.Indications.FirstAsync(x => x.SerialNumber == indicationDto.SerialNumber, cancellationToken);
                indication.Azimuth = indicationDto.Azimuth;
                indication.Elevation = indicationDto.Elevation;
                indication.WindSpeed = indicationDto.WindSpeed;
                indication.State = indicationDto.State;
            }
            else
            {
                throw new ApiException($"Indication not found", HttpStatusCode.BadRequest);
            }

            powerPlantToUpdate.Indication = powerplantIndication;

            await _applicationContext.SaveChangesAsync(cancellationToken);

            var indicationDTO = new IndicationDto(powerPlantToUpdate.SerialNumber, powerplantIndication.Azimuth, powerplantIndication.Elevation, powerplantIndication.WindSpeed, powerplantIndication.State);

            await _indicationsHub.Clients.Group(indicationDTO.SerialNumber).SendAsync("ReceiveIndication", indicationDTO);

            return indicationDTO;
        }

        public async Task<IndicationDto> AddIndicationAsync(IndicationDto indicationDto, CancellationToken cancellationToken)
        {
            var powerPlantToUpdate = await _applicationContext.Powerplants.FirstOrDefaultAsync(x => x.SerialNumber == indicationDto.SerialNumber, cancellationToken);

            if (powerPlantToUpdate == null)
            {
                throw new ApiException($"Powerplant with serial number: {indicationDto.SerialNumber} not found ", HttpStatusCode.NotFound);
            }

            if (powerPlantToUpdate.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                throw new ApiException($"Powerplant with serial number: {indicationDto.SerialNumber} is not connected ", HttpStatusCode.BadRequest);
            }

            var newIndication = new Indication()
            {
                SerialNumber = indicationDto.SerialNumber,
                Azimuth = indicationDto.Azimuth,
                Elevation = indicationDto.Elevation,
                WindSpeed = indicationDto.WindSpeed,
                State = indicationDto.State,
                Powerplant = powerPlantToUpdate,
                PowerplantId = powerPlantToUpdate.PowerplantId,
            };

            if(await _applicationContext.Indications.Where(x => x.SerialNumber == indicationDto.SerialNumber).AnyAsync(cancellationToken))
            {
                throw new ApiException($"Indication for this powerplant already exists ", HttpStatusCode.BadRequest);
            }
            else
            {
                await _applicationContext.Indications.AddAsync(newIndication, cancellationToken);
            }
            powerPlantToUpdate.Indication = newIndication;

            await _applicationContext.SaveChangesAsync(cancellationToken);

            return new IndicationDto(powerPlantToUpdate.SerialNumber, newIndication.Azimuth, newIndication.Elevation, newIndication.WindSpeed, newIndication.State);
        }

        public async Task<IndicationDto> GetIndication(string serialNumber, CancellationToken cancellationToken)
        {
            var powerPlantToUpdate = await _applicationContext.Powerplants.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber, cancellationToken);

            if (powerPlantToUpdate == null)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} not found ", HttpStatusCode.NotFound);
            }

            if (powerPlantToUpdate.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} is not connected ", HttpStatusCode.BadRequest);
            }

            var indication = await _applicationContext.Indications.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber, cancellationToken);

            if (indication == null)
            {
                throw new ApiException($"There is no powerplant indication", HttpStatusCode.NotFound);
            }

            return new IndicationDto(indication.SerialNumber, indication.Azimuth, indication.Elevation, indication.WindSpeed, indication.State);
        }

    }

        

        
}
