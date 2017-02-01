using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TasksExample.Api.Models;

namespace TasksExample.Api.Controllers
{
    public class HealthController : ApiController
    {
        /// <summary>
        /// Returns a list of health items
        /// </summary>
        /// <remarks>
        /// Returns a list of health items.  You can target a specific key by running health/{key}
        /// </remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("health")]
        [ResponseType(typeof(IEnumerable<HealthItem>))]
        public IHttpActionResult Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a specific health item based on key
        /// </summary>
        /// <param name="key">the key of the health item</param>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("health/{key}")]
        [ResponseType(typeof(HealthItem))]
        public IHttpActionResult Get(string key)
        {
            throw new NotImplementedException();
        }

    }
}
