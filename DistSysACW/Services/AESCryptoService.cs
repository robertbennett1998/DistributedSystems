using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public class AESCryptoService : IAESCryptoService
    {
        private readonly AesCryptoServiceProvider _aesCryptoServiceProvider;
        public AESCryptoService()
        {
            _aesCryptoServiceProvider = new AesCryptoServiceProvider();
            GenerateNewKey();
            GenerateNewInitialisationVector();
        }

        public void GenerateNewKey()
        {
            _aesCryptoServiceProvider.GenerateKey();
        }

        public void GenerateNewInitialisationVector()
        {
            _aesCryptoServiceProvider.GenerateIV();
        }

        public void Configure(byte[] symetricKey, byte[] initialisationVector)
        {
            _aesCryptoServiceProvider.Key = symetricKey;
            _aesCryptoServiceProvider.IV = initialisationVector;
        }

        public byte[] Encrypt(string data)
        {
            ICryptoTransform encryptor = _aesCryptoServiceProvider.CreateEncryptor();
            using (MemoryStream outStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter cryptoStreamWriter = new StreamWriter(cryptoStream))
                    {
                        cryptoStreamWriter.Write(data);
                    }
                }

                return outStream.ToArray();
            }
        }

        public string Decrypt(byte[] data)
        {
            ICryptoTransform decryptor = _aesCryptoServiceProvider.CreateDecryptor();
            using (MemoryStream inStream = new MemoryStream(data))
            {
                using (CryptoStream cryptoStream = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader cryptoStreamReader = new StreamReader(cryptoStream))
                    {
                        return cryptoStreamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
