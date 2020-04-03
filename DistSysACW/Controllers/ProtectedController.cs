using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DistSysACW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DistSysACW.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace DistSysACW.Controllers
{
    [Route("api/protected")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        private IUserService _userService;
        public ProtectedController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("hello")]
        public async Task<string> Hello_Get([FromHeader]string apiKey)
        {
            return $"Hello {(await _userService.GetUser(apiKey)).UserName}";
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("sha1")]
        public string Sha1_Get([FromHeader]string apiKey = null, [FromQuery]string message = null)
        {
            if (message == null)
                throw new BadParametersException(HttpStatusCode.BadRequest, "Bad Request");

            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("sha256")]
        public string Sha256_Get([FromHeader]string apiKey = null, [FromQuery]string message = null)
        {
            if (message == null)
                throw new BadParametersException(HttpStatusCode.BadRequest, "Bad Request");

            return BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "");
        }
    }
}