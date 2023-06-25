using EncryptionAtRest.Database.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Xunit;

namespace EncryptionAtRest.Tests
{
    public class EncryptionTests : IClassFixture<Fixture>
    {
        private readonly Fixture _fixture;

        public EncryptionTests(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task EF_ShouldEncrypt_ApiKey_InDb()
        {
            // Arrange
            var testApi = new Api
            {
                Id = Guid.NewGuid(),
                Name = "Test API",
                Key = "Test API Key"
            };


            // Act
            var db = _fixture.GetDbContext();
            db.Apis.Add(testApi);
            await db.SaveChangesAsync();


            // Assert
            // New DbContext instance is needed to get the decrypted value actually from the db
            var testApiFromDb = _fixture.GetDbContext().Apis.Find(testApi.Id);
            testApiFromDb?.Key.Should().Be(testApi.Key);

            var keyFromDbUsingRawSQL = await GetApiKeyFromDbWithoutDirectlyWithoutEF(testApi.Id);
            keyFromDbUsingRawSQL.Should().NotBeNull();
            keyFromDbUsingRawSQL.Length.Should().BeGreaterThan(0);
            keyFromDbUsingRawSQL.Should().NotBe(testApi.Key);
        }

        private async Task<string> GetApiKeyFromDbWithoutDirectlyWithoutEF(Guid id)
        {
            var db = _fixture.GetDbContext();
            using var command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = $"SELECT \"Key\" FROM \"Apis\" WHERE \"Id\" = '{id}'";
            command.CommandType = CommandType.Text;

            await db.Database.OpenConnectionAsync();
            using var result = await command.ExecuteReaderAsync();
            if (result.Read())
            {
                return result.GetString(0);
            }

            throw new Exception("No record found in the db!");
        }
    }
}