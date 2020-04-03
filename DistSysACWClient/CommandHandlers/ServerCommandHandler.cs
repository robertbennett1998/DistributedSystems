using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    public class ServerCommandHandler
    {
        private static ServerCommandHandler _instance = null;
        public static ServerCommandHandler GetInstance(UserClient client)
        {
            if (_instance == null)
                _instance = new ServerCommandHandler(client);

            return _instance;
        }

        private UserClient _userClient;
        public ServerCommandHandler(UserClient client)
        {
            _userClient = client;
        }

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

        public void SaveSettings()
        {
            _userClient.SaveSettings();
            Console.WriteLine($"Saved Settings To {_userClient.SettingsFilePath}");
        }

        public void LoadSettings()
        {
            _userClient.LoadSettings();
            Console.WriteLine($"Loaded Settings From {_userClient.SettingsFilePath}");
        }

        public void AutoSave()
        {
            Console.WriteLine($"Auto save is currently set to {_userClient.AutoSave}");
        }

        public void SetAutoSave(string autosave)
        {
            if (autosave.ToLower() == "true")
                _userClient.AutoSave = true;
            else
            if (autosave.ToLower() == "false")
                _userClient.AutoSave = false;

            Console.WriteLine($"Updated so auto save is set to {autosave}.");
        }

        public async Task Clear()
        {
            var request = _userClient.CreateRequestPath("other/clear");
            var response = await _userClient.HttpClient.GetAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
