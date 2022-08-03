using Backend.DTO.Requests;
using Backend.Services;
using Backend.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthHandler _authHandler;

        public AuthController(AuthHandler authHandler)
        {
            _authHandler = authHandler;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var response = await _authHandler.RegisterAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<RegisterResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _authHandler.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}
