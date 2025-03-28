using Microsoft.AspNetCore.Mvc;
using ProjectBackend.DTOs;
using ProjectBackend.Services;
using ProjectBackend.Models;

namespace ProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly UserRegistrationService _userRegistrationService; // Adicionando o serviço de registro

        public UserController(IUserService userService, IJwtService jwtService, UserRegistrationService userRegistrationService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _userRegistrationService = userRegistrationService; // Inicializando o serviço de registro
        }

        // Endpoint para login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        // Novo endpoint para registrar usuário
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            // Verifica se o nome de usuário e a senha são válidos
            if (string.IsNullOrEmpty(registerRequest.Username) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return BadRequest("Nome de usuário e senha são obrigatórios.");
            }

            try
            {
                // Chama o serviço para registrar o usuário
                await _userRegistrationService.RegisterUserAsync(registerRequest.Username, registerRequest.Password);
                return Ok("Usuário registrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao registrar o usuário: {ex.Message}");
            }
        }
    }
}
