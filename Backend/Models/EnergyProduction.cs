using System.Text.Json.Serialization;
namespace Backend.Models
{
    public class EnergyProduction
    {
        public int EnergyProductionId { get; set; }
        public string CurrentProduction { get; set; }
        public string DailyProduction { get; set; }
        public DateTime CurrentTime { get; set; }
        public string SerialNumber { get; set; }
        [JsonIgnore]
        public int PowerplantId { get; set; }
        [JsonIgnore]
        public Powerplant Powerplant { get; set; }

    }
}
