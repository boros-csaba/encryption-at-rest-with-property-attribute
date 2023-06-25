
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionAtRest.Database.Encryption
{
    public class EncryptedConverter : ValueConverter<string, string>
    {
        public static string EncryptionSecretKey { get; set; }

        public EncryptedConverter()
        : base(
              v => Encrypt(v),
              v => Decrypt(v))
        { }

        private static string Encrypt(string inputString)
        {
            if (string.IsNullOrWhiteSpace(EncryptionSecretKey))
                throw new ArgumentNullException(nameof(EncryptionSecretKey));

            using var aes = Aes.Create();
            using var encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(EncryptionSecretKey), aes.IV);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(inputString);
            }

            var cipher = memoryStream.ToArray();
            var result = new byte[aes.IV.Length + cipher.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(cipher, 0, result, aes.IV.Length, cipher.Length);

            return Convert.ToBase64String(result);
        }

        private static string Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(EncryptionSecretKey))
                throw new ArgumentNullException(nameof(EncryptionSecretKey));

            var cipherByteArray = Convert.FromBase64String(cipherText);
            var iv = new byte[16];
            var cipher = new byte[cipherByteArray.Length - 16];
            Buffer.BlockCopy(cipherByteArray, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(cipherByteArray, iv.Length, cipher, 0, cipher.Length);

            using var aes = Aes.Create();
            using var decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(EncryptionSecretKey), iv);
            using var memoryStream = new MemoryStream(cipher);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Adds the EncryptedConverter value converter to each property that
        /// has the [Encrypted] attribute.
        /// </summary>
        /// <param name="builder"></param>
        public static void ApplyValueConverter(ModelBuilder builder)
        {
            var entityTypes = builder.Model.GetEntityTypes();
            var properties = entityTypes.SelectMany(entity => entity.GetProperties());

            foreach (var property in properties)
            {
                var attributes = property.PropertyInfo?.GetCustomAttributes(typeof(EncryptedAttribute), true);
                if (attributes != null && attributes.Any())
                {
                    property.SetValueConverter(new EncryptedConverter());
                }
            }
        }
    }
}
