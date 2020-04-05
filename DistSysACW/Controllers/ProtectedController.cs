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
using System.Security.Claims;

namespace DistSysACW.Controllers
{
    [Route("api/protected")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICryptoService _cryptoService;

        public ProtectedController(IUserService userService, ICryptoService cryptoService)
        {
            _userService = userService;
            _cryptoService = cryptoService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("hello")]
        public string Hello_Get()
        {
            return $"Hello {HttpContext.User.FindFirst(ClaimTypes.Name).Value}";
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("sha1")]
        public string Sha1_Get([FromQuery]string message = null)
        {
            if (message == null)
                throw new BadParametersException(HttpStatusCode.BadRequest, "Bad Request");

            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("sha256")]
        public string Sha256_Get([FromQuery]string message = null)
        {
            if (message == null)
                throw new BadParametersException(HttpStatusCode.BadRequest, "Bad Request");

            return BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("getpublickey")]
        public string GetPublicKey()
        {
            return _cryptoService.GetPublicKey();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("sign")]
        public string Sign([FromQuery]string message)
        {
            return BitConverter.ToString(_cryptoService.Sign(Encoding.ASCII.GetBytes(message)));
        }
    }
}