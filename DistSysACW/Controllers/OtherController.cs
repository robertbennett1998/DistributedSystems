using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistSysACW.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/other")]
    [ApiController]
    public class OtherController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoggingService _loggingService;
        public OtherController(IUserService userService, ILoggingService loggingService)
        {
            _userService = userService;
            _loggingService = loggingService;
        }

        [HttpGet("clear")]
        public async Task<string> Clear()
        {
            await _userService.DropAllUsers();
            await _loggingService.DropAllLogs();
            return "Success, all data cleared.";
        }
    }
}