using DistSysACWClient.Attributes;
using DistSysACWClient.Services;
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
        private IUserService _userClient;
        public UserCommandHandler(IUserService client)
        {
            _userClient = client;
        }

        [Command()]
        public async Task Get(string username)
        {
            HttpResponseMessage response = await _userClient.GetAsync($"user/new?username={username}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command()]
        public async Task Post(string username)
        {
            var content = new StringContent($"\"{username}\"", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _userClient.PostAsync($"user/new", content: content);
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

        [Command()]
        public void Set(string username, string apikey)
        {
            _userClient.SetUserInfo(username, apikey);
            Console.WriteLine("Stored");
        }

        [Command("Shows the information for the currently stored user.")]
        public void Info()
        {
            Console.WriteLine("Current User Info:");
            Console.WriteLine($"\tUsername: {_userClient.Username}");
            Console.WriteLine($"\tApi Key: {_userClient.ApiKey}");
        }

        [Command()]
        public async Task Delete()
        {
            if (_userClient.Username == null || _userClient.ApiKey == "")
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            HttpResponseMessage response = await _userClient.DeleteAsync($"user/removeuser?username={_userClient.Username}", includeApiKey: true);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            _userClient.ClearUserInfo();
        }

        [Command()]
        public async Task Role(string username, string role)
        {
            if (_userClient.ApiKey == "")
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            var content = new StringContent($"{{\"username\": \"{username}\", \"{role}\": \"User\"}}");
            HttpResponseMessage response = await _userClient.PostAsync($"user/removeuser?username={_userClient.Username}", includeApiKey: true, content: content);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}