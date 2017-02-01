using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Castle.MicroKernel.Registration.Interceptor;
using MediatR;
using TasksExample.Api.Infrastructure.Data;
using TasksExample.Api.Models;

namespace TasksExample.Api.Features.Tasks
{
    public class TasksList
    {
        public class QueryAsync : IRequest<IEnumerable<TaskItem>>
        {
            public int Page { get; set; }

            public QueryAsync(int page)
            {
                Page = page;
            }
        }

        public class HandlerAsync : IAsyncRequestHandler<QueryAsync, IEnumerable<TaskItem>>
        {
            private readonly ITasksContext tasksContext;

            public HandlerAsync(ITasksContext tasksContext)
            {
                this.tasksContext = tasksContext;
            }

            public async Task<IEnumerable<TaskItem>> Handle(QueryAsync message)
            {
                // paging stuff can go here...
                return await Task.FromResult(this.tasksContext.Tasks.ToList());
            }
        }
    }
}