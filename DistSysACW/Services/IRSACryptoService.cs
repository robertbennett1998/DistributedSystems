using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface IRSACryptoService
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
