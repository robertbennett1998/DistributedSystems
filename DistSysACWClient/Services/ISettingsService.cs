using System;
using System.Collections.Generic;
using System.Text;

namespace DistSysACWClient.Services
{
    public interface ISettingsService
    {
        string SettingsFilePath { get; }

        void LoadSettings();
        void SaveSettings();
        void RegisterSettingsChangedCallback(Action<string> settingsChangedCallback);
        void UnregisterSettingsChangedCallback(Action<string> settingsChangedCallback);
        void Push<T>(string settingName, T value);
        T Pull<T>(string settingName);
    }
}
