using EncryptionAtRest.Database;
using EncryptionAtRest.Database.Models;
using FluentAssertions;
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
        public void DummyTest()
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
            var testApiFromDb = _db.Apis.Find(testApi.Id);

            
            // Assert
            testApiFromDb?.Id.Should().Be(testApi.Id);
            testApiFromDb?.Name.Should().Be(testApi.Name);
            testApiFromDb?.Key.Should().Be(testApi.Key);
        }
    }
}