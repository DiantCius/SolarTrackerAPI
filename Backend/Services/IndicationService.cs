using Backend.DataAccess;
using Backend.DTO;
using Backend.DTO.Requests;
using Backend.Errors;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;

namespace Backend.Services
{
    public class IndicationService
    {
        private readonly ApplicationContext _applicationContext;

        public IndicationService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }


        public async Task<IndicationDto> UpdateOrAddIndication(IndicationDto indicationDto, CancellationToken cancellationToken)
        {
            var powerPlantToUpdate = await _applicationContext.Powerplants.FirstAsync(x => x.SerialNumber == indicationDto.SerialNumber, cancellationToken);

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
                var indication = await _applicationContext.Indications.FirstAsync(x => x.SerialNumber == indicationDto.SerialNumber, cancellationToken);
                indication.Azimuth = indicationDto.Azimuth;
                indication.Elevation = indicationDto.Elevation;
                indication.WindSpeed = indicationDto.WindSpeed;
                indication.State = indicationDto.State;
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
            var powerPlantToUpdate = await _applicationContext.Powerplants.FirstAsync(x => x.SerialNumber == serialNumber, cancellationToken);

            if (powerPlantToUpdate == null)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} not found ", HttpStatusCode.NotFound);
            }

            if (powerPlantToUpdate.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                throw new ApiException($"Powerplant with serial number: {serialNumber} is not connected ", HttpStatusCode.BadRequest);
            }

            var indication = await _applicationContext.Indications.FirstAsync(x => x.SerialNumber == serialNumber, cancellationToken);

            return new IndicationDto(indication.SerialNumber, indication.Azimuth, indication.Elevation, indication.WindSpeed, indication.State);
        }

    }

        

        
}
