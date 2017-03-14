using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TasksExample.Api.Models
{
    public interface IRequestInfo
    {
        Guid TransactionId { get; set; }
    }

    public class RequestInfo : IRequestInfo
    {
        public Guid TransactionId { get; set; }
    }
}