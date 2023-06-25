
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

namespace EncryptionAtRest.Tests
{
    public class Fixture : IAsyncLifetime
    {
        public AppDbContext DbContext { get; }

        private readonly IContainer _dbContainer;

        public Fixture()
        {
            const int port = 5555;
            const int internalPort = 5432;
            const string user = "postgres";
            const string password = "postgres";
            const string db = "EncryptionAtRestTestDb";

            _dbContainer = new ContainerBuilder()
                .WithImage("postgres:latest")
                .WithEnvironment("POSTGRES_USER", user)
                .WithEnvironment("POSTGRES_PASSWORD", password)
                .WithEnvironment("POSTGRES_DB", db)
                .WithPortBinding(port, internalPort)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(internalPort))
                .Build();

            DbContext = new AppDbContext($"Server=localhost;Port={port};Database={db};User Id={user};Password={password};");
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}
