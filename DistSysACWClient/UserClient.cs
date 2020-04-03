using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace DistSysACWClient
{
    //Rob: 7cc33135-2caf-4669-8d32-a6c9a7aef2e1
    public class UserClient
    {

        private string _baseUri;
        public string BaseUri
        {
            get
            {
                return _baseUri;
            }
            set
            {
                _baseUri = value;
                SaveSettings();
            }
        }
        private string _username = null;
        public string Username { get => _username; private set => _username = value; }
        private string _apiKey = null;
        public string ApiKey { get => _apiKey; private set => _apiKey = value; }
        private string _settingsFilePath = null;
        public string SettingsFilePath { get => _settingsFilePath; private set => _settingsFilePath = value; }
        private bool _autoSave = true;
        public bool AutoSave { get => _autoSave; set => _autoSave = value; }

        private HttpClient _httpClient;

        public HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
        }

        public UserClient()
        {
            _httpClient = new HttpClient();
            SettingsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Robert Bennett/Distributed Systems Coursework/settings.txt";
            if (File.Exists(SettingsFilePath))
                LoadSettings();
            else
                SaveSettings();
        }

        public void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath));
            using (StreamWriter settingsFile = new StreamWriter(SettingsFilePath))
            {
                settingsFile.WriteLine($"base_uri::{BaseUri};");
                settingsFile.WriteLine($"username::{Username};");
                settingsFile.WriteLine($"api_key::{ApiKey};");
                settingsFile.WriteLine($"auto_save::{AutoSave};");
                settingsFile.Flush();
                settingsFile.Close();
            }
        }

        public void LoadSettings()
        {
            using (StreamReader settingsFile = new StreamReader(SettingsFilePath))
            {
                foreach (var line in settingsFile.ReadToEnd().Replace("\n", "").Replace("\r", "").Replace(" ", "").Split(";"))
                {
                    var parts = line.Split("::");
                    if (parts[0] == "base_uri")
                    {
                        _baseUri = parts[1];
                    }
                    else if (parts[0] == "username")
                    {
                        _username = parts[1];
                    }
                    else if (parts[0] == "api_key")
                    {
                        _apiKey = parts[1];
                    }
                    else if (parts[0] == "auto_save")
                    {
                        _autoSave = parts[1].ToLower() == "true";
                    }
                }
                settingsFile.Close();
            }
        }

        public string CreateRequestPath(string path)
        {
            return BaseUri + path;
        }

        public void SetUserInfo(string username, string apiKey)
        {
            Username = username;
            ApiKey = apiKey;

            if (AutoSave)
                SaveSettings();
        }

        public void ClearUserInfo()
        {
            Username = null;
            ApiKey = null;
        }
    }
}
