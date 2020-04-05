using DistSysACWClient.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistSysACWClient.CommandHandlers
{
    class TalkBackCommandHandler
    {
        private IUserClient _userClient;
        public TalkBackCommandHandler(IUserClient client)
        {
            _userClient = client;
        }

        [Command()]
        public async Task Hello()
        {
            var response = await _userClient.HttpClient.GetAsync(_userClient.CreateRequestPath("talkback/hello"));
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command()]
        public async Task Sort(string numbersToSort)
        {
            var request = _userClient.CreateRequestPath("talkback/sort") + "?integers=" + numbersToSort.Replace("[", "").Replace("]", "").Replace(",", "&integers=");
            var response = await _userClient.HttpClient.GetAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}