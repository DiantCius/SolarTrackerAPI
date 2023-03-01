using Backend.DataAccess;
using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Errors;
using Backend.Models;
using Backend.Services.Auth;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class AuthHandler
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly ApplicationContext _context;

        public AuthHandler(JwtGenerator jwtGenerator, ApplicationContext context)
        {
            _jwtGenerator = jwtGenerator;
            _context = context;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            if (await _context.Users.Where(x => x.Email == registerRequest.Email).AnyAsync(cancellationToken))
            {
                throw new ApiException($"User with this email already exists", HttpStatusCode.BadRequest);
            }

            if (await _context.Users.Where(x => x.Email == registerRequest.Email).AnyAsync(cancellationToken))
            {
                throw new ApiException($"User with username: {registerRequest.Email}  already exists", HttpStatusCode.BadRequest);
            }

            if(registerRequest.Password != registerRequest.ConfirmPassword)
            {
                throw new ApiException($"Both passwords must be the same", HttpStatusCode.BadRequest);
            }

            var userRole = UserRole.User;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            var newUser = new User()
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Role = userRole,
                HashedPassword = passwordHash
            };

            var token = _jwtGenerator.GenerateToken(registerRequest.Username, registerRequest.Email, userRole);

            await _context.Users.AddAsync(newUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterResponse(token);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(x => x.Email == loginRequest.Email).SingleOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                throw new ApiException("Invalid email/password", HttpStatusCode.BadRequest);
            }
            bool verified = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.HashedPassword);

            if (!verified)
            {
                throw new ApiException("Invalid email/password", HttpStatusCode.BadRequest);
            }
            var token = _jwtGenerator.GenerateToken(user.Username, user.Email, user.Role);

            return new LoginResponse(token);

        }
    }
}
