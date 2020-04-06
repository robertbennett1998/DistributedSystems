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
        private readonly ILogArchivingService _logArchivingService;
        public OtherController(IUserService userService, ILoggingService loggingService, ILogArchivingService logArchivingService)
        {
            _userService = userService;
            _loggingService = loggingService;
            _logArchivingService = logArchivingService;
        }

        [HttpGet("clear")]
        public async Task<string> Clear()
        {
            await _userService.DropAllUsers();
            await _loggingService.DropAllLogs();
            await _logArchivingService.DropAllArchivedLogs();
            return "Success, all data cleared.";
        }
    }
}