using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CoreExtensions;

namespace DistSysACWClient
{
    public class CryptoService : ICryptoService
    {
        private readonly RSACryptoServiceProvider _rsaCryptoServiceProvider;
        public CryptoService()
        {
            CspParameters cspParameters = new CspParameters();
            cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
            _rsaCryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
        }

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

        public void Configure(string publicKeyXml)
        {
            _rsaCryptoServiceProvider.FromXmlStringCore22(publicKeyXml);
        }
    }
}
