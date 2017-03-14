using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using Swashbuckle.Swagger.Annotations;
using TasksExample.Api.Features.Tasks;
using TasksExample.Api.Models;

namespace TasksExample.Api.Controllers
{
    public class TasksController : ApiController
    {
        private readonly IMediator mediator;

        public TasksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns a list of tasks
        /// </summary>
        /// <remarks>
        /// Returns a list of task items.
        /// </remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("tasks")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(ApiResponse<IEnumerable<TaskItem>>))]
        public async Task<IHttpActionResult> Get(int page = 1)
        {
            var data = await this.mediator.Send(new TasksList.QueryAsync(page));
            return this.Ok(data);
        }

        /// <summary>
        /// Returns a single task
        /// </summary>
        /// <remarks>
        /// Returns a single task item.
        /// </remarks>
        /// <response code="404">Internal Server Error</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("tasks/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(ApiResponse<TaskItem>))]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var data = await this.mediator.Send(new TaskDetail.QueryAsync(id));

            if (data == null)
            {
                return this.NotFound();
            }

            return this.Ok(data);
        }

        /// <summary>
        /// Delete a single task
        /// </summary>
        /// <remarks>
        /// Deletes a single task item.
        /// </remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete]
        [Route("tasks/{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            await this.mediator.Publish(new DeleteTask.CommandAsync(id));
            return this.StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Edit a single task
        /// </summary>
        /// <remarks>
        /// Edit a single task item.
        /// </remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("tasks/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(ApiResponse<TaskItem>))]
        public async Task<IHttpActionResult> Edit(int id, bool completed, string description)
        {
            var result = await this.mediator.Send(new EditTask.CommandAsync(id, completed,description));
            return this.Ok(result);
        }
        
        /// <summary>
        /// Create a task
        /// </summary>
        /// <remarks>
        /// Create a task item.
        /// </remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("tasks/")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(ApiResponse<TaskItem>))]
        public async Task<IHttpActionResult> Add(bool completed, string description)
        {
            var result = await this.mediator.Send(new AddTask.CommandAsync(completed, description));
            return this.Created($"tasks/{result.Id}", result);
        }

        /// <summary>
        /// Complete a task
        /// </summary>
        /// <remarks>
        /// Complete a single task item.
        /// </remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpPut]
        [Route("tasks/{id}/complete")]
        public async Task<IHttpActionResult> Complete(int id)
        {
            await this.mediator.Publish(new CompleteTask.CommandAsync(id));
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
