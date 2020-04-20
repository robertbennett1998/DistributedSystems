namespace DistSysACWClient.Services
{
    public interface IAESCryptoService
    {
        void Configure(byte[] symetricKey, byte[] initialisationVector);
        string Decrypt(byte[] data);
        byte[] Encrypt(string data);
        void GenerateNewInitialisationVector();
        void GenerateNewKey();
        byte[] GetInitialisationVector();
        byte[] GetSymmetricKey();
    }
}