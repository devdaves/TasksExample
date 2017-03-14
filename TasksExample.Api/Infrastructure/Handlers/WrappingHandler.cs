using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TasksExample.Api.Models;

namespace TasksExample.Api.Infrastructure.Handlers
{
    public class WrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (request.RequestUri.AbsolutePath.Contains("swagger"))
            {
                return response;
            }

            object content;
            ErrorResponse errorResponse = null;

            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;

                if (error != null)
                {
                    content = null;
                    errorResponse = new ErrorResponse()
                    {
                        Message = error.Message,
                        ExceptionMessage = error.ExceptionMessage,
                        ExceptionType = error.ExceptionType,
                        StackTrace = error.StackTrace
                    };
                }
            }
            var requestInfo = (IRequestInfo)request.GetDependencyScope().GetService(typeof(IRequestInfo));

            var newResponse = request.CreateResponse(response.StatusCode, new ApiResponse<object>(response.StatusCode, requestInfo.TransactionId, content, errorResponse));

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
}