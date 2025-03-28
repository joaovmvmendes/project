using ProjectBackend.Data;
using ProjectBackend.Models; // Usando o modelo User
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ProjectBackend.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password); // Usando o modelo User
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Método de autenticação
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            // Verifica se o usuário existe no banco de dados
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            
            // Se o usuário não for encontrado ou a senha for incorreta
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null; // Credenciais inválidas
            }

            return user; // Retorna o usuário
        }

        // Método que verifica se a senha fornecida é válida
        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash); // Verificação de senha com bcrypt
        }
    }
}
