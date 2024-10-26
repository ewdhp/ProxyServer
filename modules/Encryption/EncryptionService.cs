using System;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

public class EncryptionService
{
    private readonly EncryptionSettings _settings;
    private readonly Logger _logger;

    public EncryptionService(EncryptionSettings settings)
    {
        _settings = settings;
        _logger = new Logger();
    }

    public EncryptedData Encrypt(string data)
    {
        using (var aes = Aes.Create())
        {
            aes.KeySize = _settings.KeySize;
            aes.GenerateIV();
            aes.Key = GetKey();

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // Prepend IV to the output
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(data);
                        }
                    }
                    var encryptedData = new EncryptedData
                    {
                        Ciphertext = Convert.ToBase64String(ms.ToArray()),
                        Algorithm = aes.GetType().Name,
                        Timestamp = DateTime.Now
                    };
                    _logger.LogEncryptionActivity("Data encrypted successfully.");
                    return encryptedData;
                }
            }
        }
    }

    public string Decrypt(EncryptedData encryptedData)
    {
        var fullCipher = Convert.FromBase64String(encryptedData.Ciphertext);
        var iv = new byte[16]; // IV size for AES is 16 bytes
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);

        using (var aes = Aes.Create())
        {
            aes.Key = GetKey();
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        var decrypted = new StreamReader(cs).ReadToEnd();
                        _logger.LogEncryptionActivity("Data decrypted successfully.");
                        return decrypted;
                    }
                }
            }
        }
    }

    public bool ValidateToken(Token token)
    {
        var isValid = token.IsValid && token.Expiration > DateTime.Now;
        _logger.LogEncryptionActivity($"Token validation: {isValid}");
        return isValid;
    }

    public EncryptedData EncryptSession(Session session)
    {
        var sessionData = JsonConvert.SerializeObject(session);
        return Encrypt(sessionData);
    }

    private byte[] GetKey()
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(Environment.GetEnvironmentVariable("ENCRYPTION_KEY"), new byte[] { /* salt */ }, 10000))
        {
            return rfc2898.GetBytes(_settings.KeySize / 8);
        }
    }
}
