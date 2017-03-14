using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TasksExample.Api.Models
{
    public class ApiResponse<T> where T : class
    {
        public Guid TransactionId { get; set; }

        public int StatusCode { get; set; }

        public ErrorResponse Error { get; set; }

        public T Result { get; set; }

        public ApiResponse(HttpStatusCode statusCode, Guid transactionId, T result = null, ErrorResponse error = null)
        {
            this.StatusCode = (int) statusCode;
            this.TransactionId = transactionId;
            this.Result = result;
            this.Error = error;
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionType { get; set; }

        public string StackTrace { get; set; }
    }
}