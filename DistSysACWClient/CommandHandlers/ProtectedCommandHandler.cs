using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    class ProtectedCommandHandler
    {
        private static ProtectedCommandHandler _instance = null;
        public static ProtectedCommandHandler GetInstance(UserClient client)
        {
            if (_instance == null)
                _instance = new ProtectedCommandHandler(client);

            return _instance;
        }

        private UserClient _userClient;
        public ProtectedCommandHandler(UserClient client)
        {
            _userClient = client;
        }

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
    }
}
