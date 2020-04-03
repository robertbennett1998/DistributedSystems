using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class User
    {
        public enum Role
        {
            User,
            Admin
        }

        [Key]
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public Role UserRole { get; set; }
        public User()
        {
        }
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion
}