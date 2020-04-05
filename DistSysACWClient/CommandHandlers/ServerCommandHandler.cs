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
        private IClientService _userClient;
        public ServerCommandHandler(IClientService client)
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

        [Command()]
        public void SaveSettings()
        {
            throw new NotImplementedException();
        }

        [Command()]
        public void LoadSettings()
        {
            throw new NotImplementedException();
        }

        [Command("Show the current value for the auto save setting.")]
        public void AutoSave()
        {
            Console.WriteLine($"Auto save is currently set to {_userClient.AutoSave}");
        }

        [Command("Set whether or not the settings file should be automatically updated when a change to the settings is made (true/false).")]
        public void SetAutoSave(string autosave)
        {
            if (autosave.ToLower() == "true")
                _userClient.AutoSave = true;
            else
            if (autosave.ToLower() == "false")
                _userClient.AutoSave = false;

            Console.WriteLine($"Updated so auto save is set to {autosave}.");
        }

        [Command("Resets the server to it's original state.")]
        public async Task Clear()
        {
            var response = await _userClient.GetAsync("other/clear");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
