using System.Net.Http;

namespace DistSysACWClient
{
    public interface IUserClient
    {
        string ApiKey { get; }
        bool AutoSave { get; set; }
        string BaseUri { get; set; }
        HttpClient HttpClient { get; }
        string SettingsFilePath { get; }
        string Username { get; }

        void ClearUserInfo();
        string CreateRequestPath(string path);
        void LoadSettings();
        void SaveSettings();
        void SetUserInfo(string username, string apiKey);
    }
}