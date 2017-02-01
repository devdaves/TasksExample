using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TasksExample.Api.Models
{
    public class ErrorItem
    {
        public int HttpCode { get; set; }

        public int InternalCode { get; set; }

        public string Message { get; set; }
    }
}