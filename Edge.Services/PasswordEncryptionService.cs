using Edge.Services.Interfaces;
using System.Security.Cryptography;

#pragma warning disable CS8600 

namespace Edge.Services
{
    public class PasswordEncryptionService : IPasswordEncryptionService
    {
        private readonly byte[] encryptionKey;

        public PasswordEncryptionService()
        {

            string encryptionKeyString = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");

            if (string.IsNullOrEmpty(encryptionKeyString))
            {
                throw new InvalidOperationException("Encryption key environment variable is not set.");
            }

            encryptionKey = EnsureValidKeySize(Convert.FromBase64String(encryptionKeyString), 32);
        }

        private byte[] EnsureValidKeySize(byte[] key, int validKeySize)
        {
            if (key.Length == validKeySize)
            {
                return key;
            }
            else if (key.Length > validKeySize)
            {
                // If key is too long, truncate it
                byte[] truncatedKey = new byte[validKeySize];
                Array.Copy(key, truncatedKey, validKeySize);
                return truncatedKey;
            }
            else
            {
                // If key is too short, derive a valid key using a hash function (SHA-256)
                using (var sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(key);
                }
            }
        }

        public string EncryptPassword(string password)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.GenerateIV();

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(password);
                    }

                    return Convert.ToBase64String(aesAlg.IV) + ":" + Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string DecryptPassword(string encryptedPassword)
        {
            var parts = encryptedPassword.Split(':');
            var iv = Convert.FromBase64String(parts[0]);
            var cipherText = Convert.FromBase64String(parts[1]);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
