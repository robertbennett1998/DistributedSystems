using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DistSysACW.CoreExtensions;
using DistSysACW.Exceptions;
using DistSysACW.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace DistSysACW.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _userContext;
        private readonly ILogArchivingService _logArchivingService;
        public UserService(UserContext userContext, ILogArchivingService logArchivingService)
        {
            _userContext = userContext;
            _logArchivingService = logArchivingService;
        }

        public async Task AddLog(Log log, string apiKey)
        {
            var user = await GetUserByApiKey(apiKey);

            if (user == null)
                return;

            user.Logs.Add(log);
            await _userContext.SaveChangesAsync();
        }

        public async Task ChangeUserRole(string userName, string role)
        {
            var user = await GetUserByUsername(userName);

            if (user == null)
                throw new UserDoesNotExistException("NOT DONE: Username does not exist");

            object newRole = null;
            if (!Enum.TryParse(typeof(User.Role), role, out newRole))
                throw new UserRoleDoesNotExistException("NOT DONE: Role does not exist");

            user.UserRole = (User.Role)newRole;

            _userContext.Update(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task<string> CreateUser(string userName, User.Role role)
        {
            if (userName == null)
                throw new BadParametersException("Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json");

            if (await DoesUserWithUsernameExist(userName))
                throw new UserAlreadyExistsException("Oops. This username is already in use. Please try again with a new username.");

            var key = Guid.NewGuid().ToString();

            if (_userContext.Users.Count() == 0)
                role = User.Role.Admin;

            _userContext.Users.Add(new User() { UserName = userName, ApiKey = key, UserRole = role });
            await _userContext.SaveChangesAsync();
            
            return key;
        }

        public async Task<bool> DoesUserWithUsernameExist(string userName)
        {
            return await Task.Run(async () =>
                {
                    var user = await GetUserByUsername(userName);
                    return user != null;
                });
        }

        public async Task DropAllUsers()
        {
            _userContext.Users.RemoveRange(_userContext.Users);
            await _userContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByApiKey(string apiKey)
        {
            return await Task.Run(() =>
                {
                    return _userContext.Users.Include(u => u.Logs).FirstOrDefault(u => u.ApiKey == apiKey);
                });
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await Task.Run(() =>
            {
               return _userContext.Users.Include(u => u.Logs).FirstOrDefault(u => u.UserName == username);
            });
        }

        public async Task<bool> RemoveUser(string apiKey)
        {
            return await Task.Run(async () =>
            {
                var user = await GetUserByApiKey(apiKey);

                if (user == null)
                    return false;

                await _logArchivingService.ArchiveLogsForUser(user);
                _userContext.Remove(user);
                await _userContext.SaveChangesAsync();

                return true;
            });
        }
    }
}
