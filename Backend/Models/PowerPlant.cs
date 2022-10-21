
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Powerplant
    {
        public int PowerplantId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
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

    }
}
