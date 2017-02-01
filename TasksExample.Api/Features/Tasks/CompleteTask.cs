using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using TasksExample.Api.Infrastructure.Data;

namespace TasksExample.Api.Features.Tasks
{
    public class CompleteTask
    {
        public class CommandAsync : INotification
        {
            public int Id { get; set; }

            public CommandAsync(int id)
            {
                Id = id;
            }
        }

        public class HandlerAsync : IAsyncNotificationHandler<CommandAsync>
        {
            private readonly ITasksContext tasksContext;

            public HandlerAsync(ITasksContext tasksContext)
            {
                this.tasksContext = tasksContext;
            }

            public async Task Handle(CommandAsync notification)
            {
                var item = this.tasksContext.Tasks.SingleOrDefault(x => x.Id == notification.Id);

                if (item != null)
                {
                    item.Completed = true;
                }
                
                await Task.FromResult(0);
            }
        }
    }
}