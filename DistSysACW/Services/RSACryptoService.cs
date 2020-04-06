using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreExtensions;

namespace DistSysACW.Services
{
    public class RSACryptoService : IRSACryptoService
    {
        private readonly RSACryptoServiceProvider _rsaCryptoServiceProvider;
        public RSACryptoService()
        {
            CspParameters cspParameters = new CspParameters();
            cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
            _rsaCryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
        }

        public string PublicKeyXmlConfiguration { get; private set; } = null;

        public byte[] Encrypt(byte[] data)
        {
            return _rsaCryptoServiceProvider.Encrypt(data, true);
        }

        public byte[] Decrypt(byte[] data)
        {
            return _rsaCryptoServiceProvider.Decrypt(data, true);
        }

        public string GetPublicKey()
        {
            //Get only the public information
            return _rsaCryptoServiceProvider.ToXmlStringCore22(false);
        }

        public byte[] Sign(byte[] data)
        {
            return _rsaCryptoServiceProvider.SignData(data, SHA1.Create());
        }

        public bool VerifySignature(byte[] data, byte[] signature)
        {
            return _rsaCryptoServiceProvider.VerifyData(data, SHA1.Create(), signature);
        }

        public void Configure(string publicKeyXml)
        {
            _rsaCryptoServiceProvider.FromXmlStringCore22(publicKeyXml);
            PublicKeyXmlConfiguration = publicKeyXml;
        }
    }
}
