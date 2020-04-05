using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistSysACW.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherController : ControllerBase
    {
        private IUserService _userService;
        public OtherController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("clear")]
        public string Clear()
        {
            _userService.DropAllUsers();
            return "Success, all data cleared.";
        }
    }
}