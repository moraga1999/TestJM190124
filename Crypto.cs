using System.Security.Cryptography;

public class EncryptRequest
{
    public string? Text { get; set; }
}

public class DecryptRequest
{
    public string? Encrypted { get; set; }
}

public class Crypto
{
    private Aes aes;
    public Crypto(){
        aes = Aes.Create();
        aes.GenerateIV();  
        aes.GenerateKey();    
    }

    public string encrypt(string text)
    {
        ICryptoTransform encryptor = aes.CreateEncryptor();
        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(text);
                }
                byte[] bytesEncrypted = msEncrypt.ToArray();
                return Convert.ToBase64String(bytesEncrypted);
            }
        }
    }

    public string decrypt(string encrypted)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encrypted);
        ICryptoTransform decryptor = aes.CreateDecryptor();
        using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
        {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    string text = srDecrypt.ReadToEnd();
                    return text;
                }
            }
        }
        
    }
}