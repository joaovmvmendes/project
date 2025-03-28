using Microsoft.EntityFrameworkCore;
using ProjectBackend.Models;

namespace ProjectBackend.Data
{
    public class AppDbContext : DbContext
    {
        // Aqui é onde você define a DbSet para a tabela 'TaskItems'
        public DbSet<TaskItem> TaskItems { get; set; }

        // Para definir a tabela 'Users'
        public DbSet<User> Users { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

    }
}
