using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DistSysACW.Exceptions;
using DistSysACW.Models;
using Microsoft.Extensions.Primitives;

namespace DistSysACW.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _userContext;

        public UserService(UserContext userContext)
        {
            this._userContext = userContext;
        }

        public async Task ChangeUserRole(string userName, string role)
        {
            var user = await Task.Run(() => _userContext.Users.FirstOrDefault(u => u.UserName == userName));

            if (user == null)
                throw new UserDoesNotExistException("NOT DONE: Role does not exist");

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
                throw new BadParametersException("Oops. Make sure your body contains a string with your username and your Content - Type is Content - Type:application / json");

            if (await DoesUserExist(userName))
                throw new UserAlreadyExistsException("Oops.This username is already in use. Please try again with a new username.");

            var key = Guid.NewGuid().ToString();
            
            _userContext.Users.Add(new User() { UserName = userName, ApiKey = key, UserRole = role });
            await _userContext.SaveChangesAsync();
            
            return key;
        }

        public async Task<bool> DoesUserExist(string userName)
        {
            return await Task.Run(() => _userContext.Users.Any((u) => u.UserName == userName));
        }

        public async Task<User> GetUser(string apiKey)
        {
            return await Task.Run(() => _userContext.Users.FirstOrDefault(u => u.ApiKey == apiKey));
        }

        public async Task<bool> RemoveUser(string apiKey)
        {
            return await Task.Run(async () =>
            {
                var user = _userContext.Users.FirstOrDefault();

                if (user == null)
                    return false;

                _userContext.Users.Remove(user);
                await _userContext.SaveChangesAsync();

                return true;
            });
        }
    }
}
