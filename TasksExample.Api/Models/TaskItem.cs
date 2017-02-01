using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TasksExample.Api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool Completed { get; set; }
    }
}