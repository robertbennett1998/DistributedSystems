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
        Task<bool> RemoveUser(string apiKey);
        Task<bool> DoesUserWithUsernameExist(string userName);
        Task ChangeUserRole(string userName, string role);
        Task DropAllUsers();
        Task AddLog(Log log, string apiKey);
        Task<User> GetUserByApiKey(string apiKey);
        Task<User> GetUserByUsername(string username);
    }
}
