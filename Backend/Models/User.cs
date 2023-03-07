namespace Backend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public string HashedPassword { get; set; }
        public List<Powerplant> Powerplants { get; set; }
        public List<PowerplantGroup> PowerplantGroups { get; set; }

    }
}
