
namespace EncryptionAtRest.Database.Models
{
    public class Api
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Key { get; set; } = null!;
    }
}
