using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TasksExample.Api.Controllers
{
    public class HealthController : ApiController
    {
        [HttpGet]
        [Route("health")]
        public IHttpActionResult Get(string key = "")
        {
            throw new NotImplementedException();
        }
    }
}
