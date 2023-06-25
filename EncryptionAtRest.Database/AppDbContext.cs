
using EncryptionAtRest.Database.Encryption;
using EncryptionAtRest.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EncryptionAtRest.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Api> Apis { get; set; } = null!;

        public AppDbContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseNpgsql(connectionString).Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EncryptedConverter.ApplyValueConverter(modelBuilder);
            EncryptedConverter.EncryptionSecretKey = "QUFzZHBqZXBqcUBkazMxZAo=";
        }
    }
}
