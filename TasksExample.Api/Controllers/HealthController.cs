using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MediatR;
using Swashbuckle.Swagger.Annotations;
using TasksExample.Api.Features.Health;
using TasksExample.Api.Models;

namespace TasksExample.Api.Controllers
{
    public class HealthController : ApiController
    {
        private readonly IMediator mediator;

        public HealthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns a list of health items
        /// </summary>
        /// <remarks>
        /// Returns a list of health items.  You can target a specific key by running health/{key}
        /// </remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("health")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<HealthItem>))]
        public async Task<IHttpActionResult> Get()
        {
            var data = await this.mediator.Send(new HealthItemsList.QueryAsync(""));
            return this.Ok(data ?? new List<HealthItem>());
        }

        /// <summary>
        /// Returns a specific health item based on key
        /// </summary>
        /// <param name="key">the key of the health item</param>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("health/{key}")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(HealthItem))]
        public async Task<IHttpActionResult> Get(string key)
        {
            var data = await this.mediator.Send(new HealthItemsList.QueryAsync(key));

            if (data != null && data.Any())
            {
                return this.Ok(data[0]);
            }

            return this.NotFound();
        }
    }
}
