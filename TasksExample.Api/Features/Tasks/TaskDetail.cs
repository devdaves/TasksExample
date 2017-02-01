using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using TasksExample.Api.Infrastructure.Data;
using TasksExample.Api.Models;

namespace TasksExample.Api.Features.Tasks
{
    public class TaskDetail
    {
        public class QueryAsync : IRequest<TaskItem>
        {
            public int Id { get; set; }

            public QueryAsync(int id)
            {
                Id = id;
            }
        }

        public class HandlerAsync : IAsyncRequestHandler<QueryAsync, TaskItem>
        {
            private readonly ITasksContext tasksContext;

            public HandlerAsync(ITasksContext tasksContext)
            {
                this.tasksContext = tasksContext;
            }

            public async Task<TaskItem> Handle(QueryAsync message)
            {
                return await Task.FromResult(this.tasksContext.Tasks.SingleOrDefault(x => x.Id == message.Id));
            }
        }
    }
}