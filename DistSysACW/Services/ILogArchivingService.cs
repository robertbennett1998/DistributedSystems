using DistSysACW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface ILogArchivingService
    {
        Task ArchiveLogsForUser(User user);
        Task DropAllArchivedLogs();
    }
}
