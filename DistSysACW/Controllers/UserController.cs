using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using DistSysACW.Models;
using DistSysACW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DistSysACW.Controllers
{
    [Route("api/user/")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(Models.UserContext context, IUserService userService) : base(context) 
        {
            this._userService = userService;
        }

        [HttpGet("new")]
        public async Task<string> New_Get([FromQuery]string userName = null)
        {
            if (await _userService.DoesUserWithUsernameExist(userName))
                return "True - User Does Exist! Did you mean to do a POST to create a new user?";

            return "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
        }

        [HttpPost("new")]
        public async Task<string> New_Post([FromBody]string userName = null)
        {
            return await _userService.CreateUser(userName);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("removeuser")]
        public async Task<bool> RemoveUser_Delete([FromHeader]string apiKey = null, [FromQuery]string userName = null)
        {
            if ((await _userService.GetUserByApiKey(apiKey)).UserName == userName)
                return await _userService.RemoveUser(apiKey);

            return false;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("changerole")]
        public async Task<string> ChangeRole_Post([FromBody]Dictionary<string, string> body)
        {
            await _userService.ChangeUserRole(body["username"], body["role"]);
            return "DONE";
        }
    }
}
