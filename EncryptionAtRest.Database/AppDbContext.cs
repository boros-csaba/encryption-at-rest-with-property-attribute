
using EncryptionAtRest.Database.Encryption;
using EncryptionAtRest.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EncryptionAtRest.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Api> Apis { get; set; } = null!;
        private readonly string _encryptionSecretKey;

        public AppDbContext(string connectionString, string encryptionSecretKey)
            : base(new DbContextOptionsBuilder().UseNpgsql(connectionString).Options)
        {
            _encryptionSecretKey = encryptionSecretKey ?? throw new ArgumentNullException(nameof(encryptionSecretKey));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EncryptedConverter.Apply(modelBuilder, _encryptionSecretKey);
        }
    }
}
