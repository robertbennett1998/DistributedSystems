using DistSysACWClient.Attributes;
using DistSysACWClient.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DistSysACWClient.Extensions;

namespace DistSysACWClient.CommandHandlers
{
    class ProtectedCommandHandler
    {
        private readonly IUserService _userService;
        private readonly IRSACryptoService _rsaCryptoService;
        private readonly IAESCryptoService _aesCryptoService;

        public ProtectedCommandHandler(IUserService client, IRSACryptoService rsaCryptoService, IAESCryptoService aesCryptoService)
        {
            _userService = client;
            _rsaCryptoService = rsaCryptoService;
            _aesCryptoService = aesCryptoService;
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

            if (_rsaCryptoService.VerifySignature(Encoding.ASCII.GetBytes(message), (await response.Content.ReadAsStringAsync()).ConvertHexStringToBytes()))
                Console.WriteLine("Message was successfully signed");
            else
                Console.WriteLine("Message was not successfully signed");
        }

        [Command]
        public async Task AddFifty(string integer)
        {
            Console.WriteLine();
            var encryptedInteger = BitConverter.ToString(_rsaCryptoService.Encrypt(BitConverter.GetBytes(Convert.ToInt32(integer))));
            var encryptedSymKey = BitConverter.ToString(_rsaCryptoService.Encrypt(_aesCryptoService.GetSymmetricKey()));
            var encryptedIV = BitConverter.ToString(_rsaCryptoService.Encrypt(_aesCryptoService.GetInitialisationVector()));
            var response = await _userService.GetAsync($"protected/addfifty?encryptedInteger={encryptedInteger}&encryptedSymKey={encryptedSymKey}&encryptedIV={encryptedIV}", includeApiKey: true);

            if (response.StatusCode != HttpStatusCode.OK)
                    

            Console.WriteLine(_aesCryptoService.Decrypt((await response.Content.ReadAsStringAsync()).ConvertHexStringToBytes()));
        }
    }
}
