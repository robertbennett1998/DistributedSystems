using System.Net.Http;
using System.Threading.Tasks;

namespace DistSysACWClient.Services
{
    public interface IClientService
    {
        string ApiKey { get; }
        bool AutoSave { get; set; }
        string BaseUri { get; set; }
        string Username { get; }

        void ClearUserInfo();
        Task<HttpResponseMessage> DeleteAsync(string path, bool includeApiKey = false);
        Task<HttpResponseMessage> GetAsync(string path, bool includeApiKey = false);
        Task<HttpResponseMessage> PostAsync(string path, bool includeApiKey = false, HttpContent content = null);
        Task<HttpResponseMessage> SendAsync(HttpMethod verb, string path, bool includeApiKey = false, HttpContent content = null);
        void SettingsChangedCallback(string settingName);
        void SetUserInfo(string username, string apiKey);
    }
}