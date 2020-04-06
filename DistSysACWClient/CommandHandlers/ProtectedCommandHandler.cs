using DistSysACWClient.Attributes;
using DistSysACWClient.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    class ProtectedCommandHandler
    {
        private IUserService _userService;
        private ICryptoService _rsaCryptoService;
        public ProtectedCommandHandler(IUserService client, ICryptoService cryptoService)
        {
            _userService = client;
            _rsaCryptoService = cryptoService;
        }

        [Command]
        public async Task Hello()
        {
            if (_userService.ApiKey == "")
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            var response = await _userService.GetAsync("protected/hello", includeApiKey: true);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command]
        public async Task SHA1(string message)
        {
            if (_userService.ApiKey == "")
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            var response = await _userService.GetAsync($"protected/sha1?message={message}", includeApiKey: true);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command]
        public async Task SHA256(string message)
        {
            if (_userService.ApiKey == "")
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            var response = await _userService.GetAsync($"protected/sha256?message={message}", includeApiKey: true);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command]
        public async Task Get(string toGet)
        {
            if (toGet == "PublicKey")
            {
                var response = await _userService.GetAsync($"protected/getpublickey", includeApiKey: true);
                _rsaCryptoService.Configure(await response.Content.ReadAsStringAsync());

                Console.WriteLine("Got Public Key");
            }
            else
            {
                Console.WriteLine($"Invalid parameter \"{toGet}\", did you mean \"PublicKey\".");
            }
        }

        [Command]
        public async Task Sign(string message)
        {
            if (_rsaCryptoService.PublicKeyXmlConfiguration == null)
            {
                Console.WriteLine("Client doesn’t yet have the public key");
                return;
            }

            var response = await _userService.GetAsync($"protected/sign?message={message}", includeApiKey: true);

            if (_rsaCryptoService.VerifySignature(Encoding.ASCII.GetBytes(message), GetBytesFromHexString(await response.Content.ReadAsStringAsync())))
                Console.WriteLine("Message was successfully signed");
            else
                Console.WriteLine("Message was not successfully signed");
        }

        private byte[] GetBytesFromHexString(string toConvert)
        {
            List<byte> bytes = new List<byte>();
            foreach (var hexValue in toConvert.Split('-'))
            {
                bytes.Add(Convert.ToByte(hexValue, 16));
            }

            return bytes.ToArray();
        }
    }
}
