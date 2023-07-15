
using EncryptionAtRest.Database;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EncryptionAtRest
{
    class Program : IDesignTimeDbContextFactory<AppDbContext>
    {
        static void Main(string[] args)
        {
            
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var encryptionKey = configuration["EncryptionKey"] ?? throw new ArgumentNullException("EncryptionKey");
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("ConnectionString");

            return new AppDbContext(connectionString, encryptionKey);
        }
    }
}
