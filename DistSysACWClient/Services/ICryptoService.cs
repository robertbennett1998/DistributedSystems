namespace DistSysACWClient.Services
{
    public interface ICryptoService
    {
        string PublicKeyXmlConfiguration { get; }

        void Configure(string publicKeyXml);
        byte[] Decrypt(byte[] data);
        byte[] Encrypt(byte[] data);
        string GetPublicKey();
        byte[] Sign(byte[] data);
        bool VerifySignature(byte[] data, byte[] signature);
    }
}