using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface IAESCryptoService
    {
        void Configure(byte[] symetricKey, byte[] initialisationVector);
        string Decrypt(byte[] data);
        byte[] Encrypt(string data);
        void GenerateNewInitialisationVector();
        void GenerateNewKey();
    }
}
