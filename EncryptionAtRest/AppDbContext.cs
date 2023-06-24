
using EncryptionAtRest.Models;
using Microsoft.EntityFrameworkCore;

namespace EncryptionAtRest
{
    public class AppDbContext : DbContext
    {
        public DbSet<Api> Apis { get; set; } = null!;

        public AppDbContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseNpgsql(connectionString).Options)
        {
        }
    }
}
