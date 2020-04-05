using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace DistSysACWClient.Services
{
    public class SettingsService : ISettingsService
    {
        public string SettingsFilePath { get; private set; } = null;

        private Dictionary<string, string> _settings = null;

        private List<Action<string>> _settingsChangedHandlers = null;

        public SettingsService()
        {
            SettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Robert Bennett/DistributedSystemsCoursework/settings.txt";
            _settings = new Dictionary<string, string>();
            _settingsChangedHandlers = new List<Action<string>>();
        }

        public void Push<T>(string settingName, T value)
        {
            _settings[settingName] = value?.ToString();
            RaiseSettingsChangedEvent(settingName);
        }

        public T Pull<T>(string settingName)
        {
            if (_settings.ContainsKey(settingName))
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFromInvariantString(_settings[settingName]);
            }
            else
                return default(T);
        }

        /// <summary>
        /// WARNING: Removes all whitespace from the settings...
        /// </summary>
        public void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
            using (StreamWriter settingsFile = new StreamWriter(SettingsFilePath))
            {
                try
                {
                    foreach (var setting in _settings)
                        if (setting.Key != "" && setting.Value != "")
                            settingsFile.WriteLine($"{setting.Key}::={setting.Value.Replace(" ", "").Replace("\n", "").Replace("\r", "")}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                settingsFile.Flush();
                settingsFile.Close();
            }
        }

        public void LoadSettings()
        {
            if (!File.Exists(SettingsFilePath))
                return;

            using (StreamReader settingsFile = new StreamReader(SettingsFilePath))
            {
                foreach (var line in settingsFile.ReadToEnd().Replace("\r", "").Split("\n"))
                {
                    var parts = line.Split("::=", 2);
                    if (parts.Length == 2)
                        _settings[parts[0]] = parts[1];
                    else
                        _settings[parts[0]] = "";
                }
                settingsFile.Close();
            }
        }

        private void RaiseSettingsChangedEvent(string settingName)
        {
            foreach (var handler in _settingsChangedHandlers)
                handler.Invoke(settingName);
        }

        public void RegisterSettingsChangedCallback(Action<string> settingsChangedCallback)
        {
            if (_settingsChangedHandlers.Contains(settingsChangedCallback))
                return;

            _settingsChangedHandlers.Add(settingsChangedCallback);
        }

        public void UnregisterSettingsChangedCallback(Action<string> settingsChangedCallback)
        {
            if (!_settingsChangedHandlers.Contains(settingsChangedCallback))
                return;

            _settingsChangedHandlers.Remove(settingsChangedCallback);
        }
    }
}
