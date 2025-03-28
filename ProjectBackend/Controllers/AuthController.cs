using Microsoft.AspNetCore.Mvc;
using ProjectBackend.DTOs;
using ProjectBackend.Services;

namespace ProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            var user = await _userService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
