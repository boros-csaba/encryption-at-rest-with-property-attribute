using EncryptionAtRest.Database;
using EncryptionAtRest.Database.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EncryptionAtRest.Tests
{
    public class EncryptionTests : IClassFixture<Fixture>
    {
        private readonly AppDbContext _db;

        public EncryptionTests(Fixture fixture)
        {
            _db = fixture.DbContext;
        }

        [Fact]
        public async void DummyTest()
        {
            // Arrange
            var testApi = new Api
            {
                Id = Guid.NewGuid(),
                Name = "Test API",
                Key = "Test API Key"
            };


            // Act
            _db.Apis.Add(testApi);
            await _db.SaveChangesAsync();
            var testApiFromDb = _db.Apis.Find(testApi.Id);

            
            // Assert
            testApiFromDb?.Id.Should().Be(testApi.Id);
            testApiFromDb?.Name.Should().Be(testApi.Name);
            testApiFromDb?.Key.Should().Be(testApi.Key);

            var testApiFromDbRaw = _db.Apis.FromSql($"SELECT * FROM \"Apis\"").First();
            testApiFromDbRaw?.Id.Should().Be(testApi.Id);
            testApiFromDbRaw?.Name.Should().Be(testApi.Name);
            testApiFromDbRaw?.Key.Should().Be(testApi.Key);
        }
    }
}