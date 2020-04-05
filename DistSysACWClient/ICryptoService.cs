namespace DistSysACWClient
{
    public interface ICryptoService
    {
        void Configure(string publicKeyXml);
        byte[] Decrypt(byte[] data);
        byte[] Encrypt(byte[] data);
        string GetPublicKey();
    }
}