using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using TasksExample.Api.Infrastructure.Data;
using TasksExample.Api.Models;

namespace TasksExample.Api.Features.Tasks
{
    public class AddTask
    {
        public class CommandAsync : IRequest<TaskItem>
        {
            public bool Completed { get; set; }
            public string Description { get; set; }

            public CommandAsync(bool completed, string description)
            {
                Completed = completed;
                Description = description;
            }
        }

        public class HandlerAsync : IAsyncRequestHandler<CommandAsync, TaskItem>
        {
            private readonly ITasksContext tasksContext;

            public HandlerAsync(ITasksContext tasksContext)
            {
                this.tasksContext = tasksContext;
            }

            public async Task<TaskItem> Handle(CommandAsync message)
            {
                var nextId = 1;
                if (this.tasksContext.Tasks.Any())
                {
                    nextId = this.tasksContext.Tasks.Max(x => x.Id) + 1;
                }

                var item = new TaskItem()
                {
                    Id = nextId,
                    Completed = message.Completed,
                    Description = message.Description
                };

                this.tasksContext.Tasks.Add(item);

                return await Task.FromResult(item);
            }
        }
    }

}