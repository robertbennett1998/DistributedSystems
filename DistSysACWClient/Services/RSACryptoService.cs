using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using CoreExtensions;

namespace DistSysACWClient.Services
{
    public class RSACryptoService : IRSACryptoService
    {
        private readonly RSACryptoServiceProvider _rsaCryptoServiceProvider;
        private readonly ISettingsService _settingsService;
        public RSACryptoService(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            CspParameters cspParameters = new CspParameters();
            cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
            _rsaCryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
        }

        public string PublicKeyXmlConfiguration 
        { 
            get
            {
                return _settingsService.Pull<string>("PublicKeyConfig");
            }

            private set
            {
                _settingsService.Push("PublicKeyConfig", value);
                _settingsService.SaveSettings();
            }
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
            try
            {
                _rsaCryptoServiceProvider.FromXmlStringCore22(publicKeyXml);
                PublicKeyXmlConfiguration = publicKeyXml;
            }
            catch (XmlException)
            {
                PublicKeyXmlConfiguration = "";
            }
        }
    }
}
