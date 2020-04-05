using DistSysACWClient.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    class ProtectedCommandHandler
    {
        private IUserClient _userClient;
        private ICryptoService _cryptoService;
        public ProtectedCommandHandler(IUserClient client, ICryptoService cryptoService)
        {
            _userClient = client;
            _cryptoService = cryptoService;
        }

        [Command]
        public async Task Hello()
        {
            if (_userClient.ApiKey == null)
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _userClient.CreateRequestPath("protected/hello"));
            request.Headers.Add("ApiKey", _userClient.ApiKey);

            var response = await _userClient.HttpClient.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command]
        public async Task SHA1(string message)
        {
            if (_userClient.ApiKey == null)
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _userClient.CreateRequestPath($"protected/sha1?message={message}"));
            request.Headers.Add("ApiKey", _userClient.ApiKey);

            var response = await _userClient.HttpClient.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command]
        public async Task SHA256(string message)
        {
            if (_userClient.ApiKey == null)
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _userClient.CreateRequestPath($"protected/sha256?message={message}"));
            request.Headers.Add("ApiKey", _userClient.ApiKey);

            var response = await _userClient.HttpClient.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command]
        public async Task Get(string toGet)
        {
            if (toGet == "PublicKey")
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _userClient.CreateRequestPath($"protected/getpublickey"));
                request.Headers.Add("ApiKey", _userClient.ApiKey);

                var response = await _userClient.HttpClient.SendAsync(request);
                var publicKeyXml = await response.Content.ReadAsStringAsync();
                _cryptoService.Configure(publicKeyXml);

                Console.WriteLine("Got Public Key");
            }
            else
            {
                Console.WriteLine($"Invalid parameter \"{toGet}\", did you mean \"PublicKey\".");
            }
        }
    }
}
