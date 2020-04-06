using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface ILoggingService
    {
        Task LogAuthorisedRequest(string message, string apiKey);
        Task LogAuthorisedRequest(string apiKey, string userName, string role, string verb, string path);
        Task DropAllLogs();
    }
}
