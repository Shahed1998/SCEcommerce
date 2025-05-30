using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Helpers
{
    public class HelperEncryption
    {
        private readonly byte[] _key;

        public HelperEncryption(IConfiguration configuration)
        {
            var encryptionSetting = configuration.GetSection("EncryptionSettings");

            var test = encryptionSetting["Key"];
            _key = Encoding.UTF8.GetBytes(encryptionSetting["Key"] ?? "");
        }

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;

                // Generate a random IV
                aesAlg.GenerateIV();
                byte[] iv = aesAlg.IV;

                // Create encryptor
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);

                // Convert text to bytes
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // Combine IV + encrypted bytes
                byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, combinedBytes, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);

                return Convert.ToBase64String(combinedBytes);
            }
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                byte[] combinedBytes = Convert.FromBase64String(encryptedText);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = _key;

                    // Extract IV (first 16 bytes)
                    byte[] iv = new byte[16];
                    Buffer.BlockCopy(combinedBytes, 0, iv, 0, iv.Length);

                    // Extract actual encrypted data (remaining bytes)
                    int encryptedLength = combinedBytes.Length - iv.Length;
                    byte[] encryptedBytes = new byte[encryptedLength];
                    Buffer.BlockCopy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedLength);

                    aesAlg.IV = iv;

                    // Create decryptor
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception ex)
            {
                HelperSerilog.LogError(ex.Message, ex);
                return string.Empty;
            }
        }
    }
}
