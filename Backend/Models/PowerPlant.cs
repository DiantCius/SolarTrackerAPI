
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class PowerPlant
    {
        public int PowerPlantId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public PowerPlantType PowerPlantType { get; set; }
        public string SerialNumber { get; set; }
        public ConnectionStatus ConnectionStatus { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

    }
}
