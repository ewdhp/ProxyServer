public class EncryptedData
{
    public string? Ciphertext { get; set; }  // Base64 encoded ciphertext
    public string? Iv { get; set; }            // Base64 encoded IV
    public string? Algorithm { get; set; }     // Encryption algorithm used
    public DateTime Timestamp { get; set; }   // Timestamp of encryption
}
