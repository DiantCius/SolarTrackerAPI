using Microsoft.IdentityModel.Tokens;

namespace Backend
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public DateTime NotBefore = DateTime.UtcNow;
        public DateTime Expires = DateTime.UtcNow.AddHours(1.0);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
