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
        private IUserService _userClient;
        public TalkBackCommandHandler(IUserService client)
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
            var numbers = numbersToSort.Replace("[", "").Replace("]", "").Split(",");
            string request = "talkback/sort";
            bool firstNumber = true;
            foreach (var number in numbers)
            {
                if (number == "")
                    continue;

                if (firstNumber)
                {
                    request += "?integers=" + number;
                    firstNumber = false;
                    continue;
                }
                
                request += "&integers=" + number;
            }

            var response = await _userClient.GetAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Bad Request");
                return;
            }

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}