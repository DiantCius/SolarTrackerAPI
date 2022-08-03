using Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Services
{
    public class JwtGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public JwtGenerator(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public string GenerateToken(string username, string email, UserRole userRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userRole.ToString())
            };

            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expires,
                _jwtOptions.SigningCredentials
                );

            return tokenHandler.WriteToken(jwt);
        }
    }
}
