
using Microsoft.EntityFrameworkCore.Design;

namespace EncryptionAtRest
{
    class Program : IDesignTimeDbContextFactory<AppDbContext>
    {
        static void Main(string[] args)
        {

        }

        public AppDbContext CreateDbContext(string[] args)
        {
            return new AppDbContext("test");
        }
    }
}
