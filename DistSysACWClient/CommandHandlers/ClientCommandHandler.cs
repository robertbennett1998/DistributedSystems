using DistSysACWClient.Attributes;
using DistSysACWClient.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistSysACWClient.CommandHandlers
{
    class ClientCommandHandler
    {
        private IUserService _userClient;
        private ISettingsService _settingsService;
        public ClientCommandHandler(IUserService client, ISettingsService settingsService)
        {
            _userClient = client;
            _settingsService = settingsService;
        }

        [Command]
        public void SaveSettings()
        {
            if (_settingsService.SaveSettings())
            {
                Console.WriteLine("Saved settings.");
            }
            else
            {
                Console.WriteLine("Failed to save settings.");
            }
        }

        [Command]
        public void LoadSettings()
        {
            if (_settingsService.LoadSettings();)
            {
                Console.WriteLine("Loaded settings.");
            }
            else
            {
                Console.WriteLine("Failed to load settings.");
            }
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
    }
}
