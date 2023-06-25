
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using EncryptionAtRest.Database;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EncryptionAtRest.Tests
{
    public class Fixture : IAsyncLifetime
    {
        public AppDbContext DbContext { get; }

        private readonly IContainer _dbContainer;

        const int port = 5555;
        const int internalPort = 5432;
        const string user = "postgres";
        const string password = "postgres";
        const string db = "EncryptionAtRestTestDb";

        public Fixture()
        {
            _dbContainer = CreateDbContainer();
            DbContext = new AppDbContext($"Server=localhost;Port={port};Database={db};User Id={user};Password={password};");
        }

        private IContainer CreateDbContainer()
        {
            return new ContainerBuilder()
                .WithImage("postgres:latest")
                .WithEnvironment("POSTGRES_USER", user)
                .WithEnvironment("POSTGRES_PASSWORD", password)
                .WithEnvironment("POSTGRES_DB", db)
                .WithPortBinding(port, internalPort)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(internalPort))
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            DbContext.Database.Migrate();
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}
