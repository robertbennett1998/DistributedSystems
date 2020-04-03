using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DistSysACWClient
{
    public class UserClient
    {
        private string _baseUri = null;

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
            _baseUri = baseUri;

            _httpClient = new HttpClient();
        }

        public string CreateRequestPath(string path)
        {
            return _baseUri + path;
        }
    }
}
