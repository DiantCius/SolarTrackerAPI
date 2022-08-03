namespace Backend.Models
{
    public class SolarTrackerIndication
    {
        public string SerialNumber { get; set; }
        public float Azimuth { get; set; }
        public float Elevation { get; set; }
        public float WindSpeed { get; set; }
        public int[] State { get; set; }
    }
}
