
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Powerplant
    {
        public int PowerplantId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Tariff { get; set; }
        public PowerplantType PowerplantType { get; set; }
        public string SerialNumber { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; }
        [JsonIgnore]
        public List<EnergyProduction> EnergyProductions { get; set; }
        [JsonIgnore]
        public Indication Indication { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public int PowerplantGroupId { get; set; }
        [JsonIgnore]
        public PowerplantGroup PowerplantGroup { get; set; }

    }
}
