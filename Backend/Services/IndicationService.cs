using Backend.DataAccess;
using Backend.Errors;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class IndicationService
    {
        private readonly ApplicationContext _applicationContext;

        public IndicationService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task UpdateSolarTrackerIndication(SolarTrackerIndication solarTrackerIndication, CancellationToken cancellationToken)
        {
            var powerPlantToUpdate = await _applicationContext.Powerplants.FirstAsync(x => x.SerialNumber == solarTrackerIndication.SerialNumber, cancellationToken);

            if (powerPlantToUpdate == null)
            {
                throw new ApiException($"Powerplant with serial number: {solarTrackerIndication.SerialNumber} not found ", HttpStatusCode.NotFound);
            }

            if(powerPlantToUpdate.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                throw new ApiException($"Powerplant with serial number: {solarTrackerIndication.SerialNumber} is not connected ", HttpStatusCode.BadRequest);
            }

            if(Indications.GetSolarTrackerIndication(solarTrackerIndication.SerialNumber) == null && powerPlantToUpdate.ConnectionStatus == ConnectionStatus.Connected)
            {
                AddSolarTrackerIndication(solarTrackerIndication);
            }

            Indications.UpdateTrackerIndication(solarTrackerIndication);
            

        }

        public SolarTrackerIndication GetSolarTrackerIndication(string serialNumber)
        {
            var solarTrackerIndication =  Indications.solarTrackerIndications.Where(x => x.SerialNumber == serialNumber).FirstOrDefault();
            if (solarTrackerIndication == null)
            {
                throw new ApiException($"Indication with serial number: {serialNumber} not found ", HttpStatusCode.NotFound);
            }
            return solarTrackerIndication;
        }

        public static void AddSolarTrackerIndication(SolarTrackerIndication solarTrackerIndication)
        {
            Indications.AddSolarTrackerIndication(solarTrackerIndication);
        }
    }
}
