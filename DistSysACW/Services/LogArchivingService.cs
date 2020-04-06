using DistSysACW.CoreExtensions;
using DistSysACW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public class LogArchivingService : ILogArchivingService
    {
        private readonly UserContext _userContext;
        public LogArchivingService(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task ArchiveLogsForUser(User user)
        {
            await _userContext.LogArchives.AddRangeAsync(user.Logs.Apply((log) => LogArchive.FromLog(log)));
            await _userContext.SaveChangesAsync();
        }

        public async Task DropAllArchivedLogs()
        {
            _userContext.LogArchives.RemoveRange(_userContext.LogArchives);
            await _userContext.SaveChangesAsync();
        }
    }
}
