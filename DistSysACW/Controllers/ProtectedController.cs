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
using DistSysACW.Extensions;

namespace DistSysACW.Controllers
{
    [Route("api/protected")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRSACryptoService _rsaCryptoService;
        private readonly IAESCryptoService _aesCryptoService;

        public ProtectedController(IUserService userService, IRSACryptoService rsaCryptoService, IAESCryptoService aesCryptoService)
        {
            _userService = userService;
            _rsaCryptoService = rsaCryptoService;
            _aesCryptoService = aesCryptoService;
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

            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(message))).Replace("-", "");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("sha256")]
        public string Sha256_Get([FromQuery]string message = null)
        {
            if (message == null)
                throw new BadParametersException(HttpStatusCode.BadRequest, "Bad Request");

            return BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(message))).Replace("-", "");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("getpublickey")]
        public string GetPublicKey_Get()
        {
            return _rsaCryptoService.GetPublicKey();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("sign")]
        public string Sign_Get([FromQuery]string message)
        {
            return BitConverter.ToString(_rsaCryptoService.Sign(Encoding.ASCII.GetBytes(message)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("addfifty")]
        public string AddFifty_Get([FromQuery]string encryptedInteger, [FromQuery]string encryptedSymKey, [FromQuery]string encryptedIV)
        {
            int decrytedInteger = BitConverter.ToInt32(_rsaCryptoService.Decrypt(encryptedInteger.ConvertHexStringToBytes()));
            _aesCryptoService.Configure(_rsaCryptoService.Decrypt(encryptedSymKey.ConvertHexStringToBytes()), _rsaCryptoService.Decrypt(encryptedIV.ConvertHexStringToBytes()));
            string encryptedIntegerPlusFifty = BitConverter.ToString(_aesCryptoService.Encrypt(Convert.ToString(decrytedInteger + 50)));

            return encryptedIntegerPlusFifty;
        }
    }
}