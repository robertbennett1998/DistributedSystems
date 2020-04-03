using DistSysACW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Services
{
    public interface IUserService
    {
        Task<string> CreateUser(string userName, User.Role role = User.Role.User);
        Task<User> GetUser(string apiKey);
        Task<bool> RemoveUser(string apiKey);
        Task<bool> DoesUserExist(string userName);
        Task ChangeUserRole(string userName, string role);
    }
}
