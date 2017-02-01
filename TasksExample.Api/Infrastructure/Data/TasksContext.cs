using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TasksExample.Api.Models;

namespace TasksExample.Api.Infrastructure.Data
{
    public interface ITasksContext
    {
        List<TaskItem> Tasks { get; set; }
    }

    public class TasksContext : ITasksContext
    {
        public virtual List<TaskItem> Tasks { get; set; }

        public TasksContext()
        {
            this.Tasks = new List<TaskItem>()
            {
                new TaskItem() { Id = 1, Completed = false, Description = "Test Task 1"},
                new TaskItem() { Id = 2, Completed = false, Description = "Test Task 2"},
                new TaskItem() { Id = 3, Completed = false, Description = "Test Task 3"},
                new TaskItem() { Id = 4, Completed = false, Description = "Test Task 4"},
                new TaskItem() { Id = 5, Completed = false, Description = "Test Task 5"},
            };
        }
    }
}