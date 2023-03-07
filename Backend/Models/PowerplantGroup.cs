namespace Backend.Models
{
    public class PowerplantGroup
    {
        public int PowerplantGroupId { get; set; }
        public string Name { get; set; }
        public List<Powerplant> Powerplants { get; set; }
    }
}
