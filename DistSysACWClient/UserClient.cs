using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DistSysACWClient
{
    //Rob: 7cc33135-2caf-4669-8d32-a6c9a7aef2e1
    public class UserClient
    {
        public string BaseUri { get; set; }
        public string Username { get; private set; } = null;
        public string ApiKey { get; private set; } = null;

        private HttpClient _httpClient;
        public HttpClient HttpClient
        { 
            get 
            {
                return _httpClient;
            }
        }

        public UserClient(string baseUri)
        {
            BaseUri = baseUri;

            _httpClient = new HttpClient();
        }

        public string CreateRequestPath(string path)
        {
            return BaseUri + path;
        }

        public void SetUserInfo(string username, string apiKey)
        {
            Username = username;
            ApiKey = apiKey;
        }

        public void ClearUserInfo()
        {
            Username = null;
            ApiKey = null;
        }
    }
}
