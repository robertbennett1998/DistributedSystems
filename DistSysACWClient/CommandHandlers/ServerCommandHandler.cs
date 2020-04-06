using DistSysACWClient.Attributes;
using DistSysACWClient.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    public class ServerCommandHandler
    {
        private IUserService _userClient;
        public ServerCommandHandler(IUserService client)
        {
            _userClient = client;
        }

        [Command("Set the server URI. Can enter anything or there are two pre-configured settings:\n\t\tlocalhost - https://localhost:44307/api/\n\t\ttestserver - http://distsysacw.azurewebsites.net/6170585/api/")]
        public void SetBaseUri(string uri)
        {
            if (uri.ToLower() == "testserver")
            {
                uri = "http://distsysacw.azurewebsites.net/6170585/api/";
            }
            else if (uri.ToLower() == "localhost")
            {
                uri = "https://localhost:44307/api/";
            }

            _userClient.BaseUri = uri;
            Console.WriteLine($"Base URI updated. New URI is {_userClient.BaseUri}.");
        }

        [Command("Shows the base URI that is currently being used.")]
        public void BaseUri()
        {
            Console.WriteLine($"The base URI is currently {_userClient.BaseUri}.");
        }

        [Command("Resets the server to it's original state.")]
        public async Task Clear()
        {
            var response = await _userClient.GetAsync("other/clear");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
