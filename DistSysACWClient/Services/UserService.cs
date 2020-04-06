using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACWClient.Services
{
    public class UserService : IUserService
    {
        public string BaseUri
        {
            get
            {
                return _settingsService.Pull<string>("BaseUri");
            }

            set
            {
                _settingsService.Push("BaseUri", value);
                _settingsService.SaveSettings();
            }
        }

        public string Username
        {
            get
            {
                return _settingsService.Pull<string>("Username");
            }

            private set
            {
                _settingsService.Push("Username", value);
                _settingsService.SaveSettings();
            }
        }

        public string ApiKey
        {
            get
            {
                return _settingsService.Pull<string>("ApiKey");
            }

            private set
            {
                _settingsService.Push("ApiKey", value);
                _settingsService.SaveSettings();
            }
        }

        public bool AutoSave
        {
            get
            {
                return _settingsService.Pull<string>("AutoSave").ToLower() == "true";
            }

            set
            {
                _settingsService.Push("AutoSave", value ? "true" : "false");
                _settingsService.SaveSettings();
            }
        }

        private readonly HttpClient _httpClient;
        private readonly ISettingsService _settingsService;
        public UserService(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            _httpClient = new HttpClient();
        }

        public void SetUserInfo(string username, string apiKey)
        {
            Username = username;
            ApiKey = apiKey;
        }

        public void ClearUserInfo()
        {
            Username = "";
            ApiKey = "";
        }

        public void SettingsChangedCallback(string settingName)
        {

        }

        public async Task<HttpResponseMessage> GetAsync(string path, bool includeApiKey)
        {
            return await SendAsync(HttpMethod.Get, path, includeApiKey);
        }

        public async Task<HttpResponseMessage> PostAsync(string path, bool includeApiKey, HttpContent content)
        {
            return await SendAsync(HttpMethod.Post, path, includeApiKey, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string path, bool includeApiKey)
        {
            return await SendAsync(HttpMethod.Delete, path, includeApiKey);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod verb, string path, bool includeApiKey, HttpContent content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(verb, BaseUri + path);
            if (includeApiKey)
                request.Headers.Add("ApiKey", ApiKey);

            if (content != null)
                request.Content = content;

            return await _httpClient.SendAsync(request);
        }
    }
}
