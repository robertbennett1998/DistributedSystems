using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    public class UserCommandHandler
    {
        private static UserCommandHandler _instance = null;
        public static UserCommandHandler GetInstance(UserClient client)
        {
            if (_instance == null)
                _instance = new UserCommandHandler(client);

            return _instance;
        }

        private UserClient _userClient;
        public UserCommandHandler(UserClient client)
        {
            _userClient = client;
        }

        public async Task Get(string username)
        {
            var request = _userClient.CreateRequestPath("user/new") + "?username=" + username;
            var response = await _userClient.HttpClient.GetAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public async Task Post(string username)
        {
            var request = _userClient.CreateRequestPath("user/new");
            var content = new StringContent($"\"{username}\"", Encoding.UTF8, "application/json");
            var response = await _userClient.HttpClient.PostAsync(request, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"Got API Key");
                _userClient.SetUserInfo(username, await response.Content.ReadAsStringAsync());
            }
            else
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }

        public void Set(string username, string apikey)
        {
            _userClient.SetUserInfo(username, apikey);
            Console.WriteLine("Stored");
        }

        public void Info()
        {
            Console.WriteLine("Current User Info:");
            Console.WriteLine($"\tUsername: {_userClient.Username}");
            Console.WriteLine($"\tApi Key: {_userClient.ApiKey}");
        }

        public async Task Delete()
        {
            if (_userClient.Username == null || _userClient.ApiKey == null)
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, _userClient.CreateRequestPath("user/removeuser") + "?username=" + _userClient.Username);
            request.Headers.Add("ApiKey", _userClient.ApiKey);

            var response = await _userClient.HttpClient.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            _userClient.ClearUserInfo();
        }

        public async Task Role(string username, string role)
        {
            if (_userClient.ApiKey == null)
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _userClient.CreateRequestPath("user/changerole"));
            request.Headers.Add("ApiKey", _userClient.ApiKey);
            request.Content = new StringContent($"{{\"username\": \"{username}\", \"{role}\": \"User\"}}");

            var response = await _userClient.HttpClient.SendAsync(request);

            //if (response.StatusCode != HttpStatusCode.OK)
            //{
            //    Console.WriteLine($"Request not ok. Error code {response.StatusCode} - {response.StatusCode.ToString()}.");
            //}

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}