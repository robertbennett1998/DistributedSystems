using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    class TalkBack
    {
        private static TalkBack _instance = null;
        public static TalkBack GetInstance(UserClient client)
        {
            if (_instance == null)
                _instance = new TalkBack(client);

            return _instance;
        }

        private UserClient _userClient;

        private TalkBack(UserClient client)
        {
            _userClient = client;
        }

        public async Task Hello()
        {
            var response = await _userClient.HttpClient.GetAsync(_userClient.CreateRequestPath("talkback/hello"));
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public async Task Sort(string numbersToSort)
        {
            var request = _userClient.CreateRequestPath("talkback/sort") + "?integers=" + numbersToSort.Replace("[", "").Replace("]", "").Replace(",", "&integers=");
            var response = await _userClient.HttpClient.GetAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}