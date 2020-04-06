using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class User
    {

        public User()
        {
            Logs = new List<Log>();
        }
        public enum Role
        {
            User,
            Admin
        }

        [Key]
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public Role UserRole { get; set; }
        public List<Log> Logs { get; set; }
    }
}