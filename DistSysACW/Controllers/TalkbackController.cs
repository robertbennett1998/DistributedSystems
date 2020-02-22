using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    public class TalkBackController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkBackController(Models.UserContext context) : base(context) { }


        [HttpGet]
        public string Hello()
        {
            #region TASK1
            return "Hello World";
            // TODO: add api/talkback/hello response
            #endregion
        }

        [HttpGet]
        public IActionResult Sort([FromQuery]int[] integers)
        {
            #region TASK1
            Array.Sort(integers);
            return Ok(integers);
            // TODO: 
            // sort the integers into ascending order
            // send the integers back as the api/talkback/sort response
            #endregion
        }
    }
}
