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
    public class EditTask
    {
        public class CommandAsync : IRequest<TaskItem>
        {
            public int Id { get; set; }
            public bool Completed { get; set; }
            public string Description { get; set; }

            public CommandAsync(int id, bool completed, string description)
            {
                Id = id;
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
                var item = this.tasksContext.Tasks.SingleOrDefault(x => x.Id == message.Id);

                if (item != null)
                {
                    item.Completed = message.Completed;
                    item.Description = message.Description;
                }

                return await Task.FromResult(item);
            }
        }
    }
}