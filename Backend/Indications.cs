using Backend.Models;

namespace Backend
{
    public static class Indications
    {
        public static List<SolarTrackerIndication> solarTrackerIndications = new List<SolarTrackerIndication>();

        public static void AddSolarTrackerIndication(SolarTrackerIndication solarTrackerIndication)
        {
            solarTrackerIndications.Add(solarTrackerIndication);
        }

        public static void UpdateTrackerIndication(SolarTrackerIndication solarTrackerIndication)
        {
            var index = solarTrackerIndications.FindIndex(x=> x.SerialNumber == solarTrackerIndication.SerialNumber);
            solarTrackerIndications[index] = solarTrackerIndication;
        }

        public static SolarTrackerIndication GetSolarTrackerIndication(string serialNumber)
        {
            return solarTrackerIndications.Where(x=> x.SerialNumber == serialNumber).FirstOrDefault();
        }

    }
}
