using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class PowerplantGroup
    {
        public int PowerplantGroupId { get; set; }
        public string Name { get; set; }
        public List<Powerplant> Powerplants { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
