using BCrypt.Net;
using ProjectBackend.Models;
using ProjectBackend.Data;

namespace ProjectBackend.Services
{
    public class UserRegistrationService
    {
        private readonly AppDbContext _context;

        public UserRegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUserAsync(string username, string password)
        {   
            // Criptografa a senha usando BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);         

            // Cria o objeto User com a senha criptografada
            var user = new User
            {
                Username = username,
                Password = password,
                PasswordHash = hashedPassword
            };

            // Salva o usuário no banco de dados
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
