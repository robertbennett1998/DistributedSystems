using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class LogArchive
    {

        public LogArchive()
        {

        }

        [Key]
        public int LogArchiveId { get; set; }
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }
        public string UserApiKey { get; internal set; }

        public static LogArchive FromLog(Log log)
        {
            LogArchive logArchive = new LogArchive();
            logArchive.LogString = log.LogString;
            logArchive.LogDateTime = log.LogDateTime;
            logArchive.UserApiKey = log.UserApiKey;

            return logArchive;
        }
    }
}
