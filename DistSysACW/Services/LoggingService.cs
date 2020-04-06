using DistSysACW.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly UserContext _userContext;
        private readonly IUserService _userService;
        public LoggingService(UserContext userContext, IUserService userService)
        {
            _userContext = userContext;
            _userService = userService;
        }

        public async Task DropAllLogs()
        {
            _userContext.Logs.RemoveRange(_userContext.Logs);
            await _userContext.SaveChangesAsync();
        }

        public async Task LogAuthorisedRequest(string apiKey, string userName, string role, string verb, string path)
        {
            var logMessage = $"{role} {userName} made a ({verb}) request to {path}.";
            await _userService.AddLog(new Log(logMessage), apiKey);
            Debug.WriteLine(logMessage);
        }

        public async Task LogAuthorisedRequest(string message, string apiKey)
        {
            await _userService.AddLog(new Log(message), apiKey);
        }
    }
}
