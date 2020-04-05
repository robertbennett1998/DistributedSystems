using DistSysACWClient.Attributes;
using DistSysACWClient.Services;
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
        private IClientService _userClient;
        public TalkBackCommandHandler(IClientService client)
        {
            _userClient = client;
        }

        [Command()]
        public async Task Hello()
        {
            var response = await _userClient.GetAsync("talkback/hello");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        [Command()]
        public async Task Sort(string numbersToSort)
        {
            var response = await _userClient.GetAsync($"talkback/sort?integers={ numbersToSort.Replace("[", "").Replace("]", "").Replace(",", "&integers=")}");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}