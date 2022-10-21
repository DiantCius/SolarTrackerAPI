using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Indication
    {
        public int IndicationId { get; set; }
        public string SerialNumber { get; set; }
        public float Azimuth { get; set; }
        public float Elevation { get; set; }
        public float WindSpeed { get; set; }
        public int[] State { get; set; }
        public int PowerplantId { get; set; }
        public Powerplant Powerplant { get; set; }
    }
}

