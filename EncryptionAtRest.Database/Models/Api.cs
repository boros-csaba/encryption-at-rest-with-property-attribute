
using EncryptionAtRest.Database.Encryption;

namespace EncryptionAtRest.Database.Models
{
    public class Api
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        [Encrypted]
        public string Key { get; set; } = null!;
    }
}
