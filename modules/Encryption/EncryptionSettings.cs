public class EncryptionSettings
{
    public string DefaultAlgorithm { get; set; }  // e.g., "AES"
    public int KeySize { get; set; }               // Key size in bits
    public int IvSize { get; set; }                // IV size in bits
}
