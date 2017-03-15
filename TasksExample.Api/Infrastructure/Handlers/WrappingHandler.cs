using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Castle.Core.Internal;
using Castle.Core.Logging;
using TasksExample.Api.Models;

namespace TasksExample.Api.Infrastructure.Handlers
{
    public class WrappingHandler : DelegatingHandler
    {
        private IRequestInfo requestInfo;
        private ILogger logger;

        public WrappingHandler()
        {
            // needed by asp.net since DI doesn;t work for MessageHandlers
        }

        public WrappingHandler(IRequestInfo requestInfo, ILogger logger)
        {
            this.requestInfo = requestInfo;
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (this.requestInfo == null)
            {
                this.requestInfo = (IRequestInfo)request.GetDependencyScope().GetService(typeof(IRequestInfo));
            }

            if (this.logger == null)
            {
                this.logger = (ILogger)request.GetDependencyScope().GetService(typeof(ILogger));
            }

            return this.BuildApiResponse(request, response);
        }

        private HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (this.IsSwagger(request))
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

                    this.logger.Error($"{this.requestInfo.TransactionId} - Error: {error.Message} | {error.ExceptionMessage} | {error.StackTrace}");
                }
            }

            var newResponse = request.CreateResponse(response.StatusCode,
                new ApiResponse<object>(response.StatusCode, requestInfo.TransactionId, content, errorResponse));

            response.Headers.ForEach(h => newResponse.Headers.Add(h.Key, h.Value));

            // not needed, just an example of logging all responses
            this.logger.Debug($"{requestInfo.TransactionId} - Returned response");

            return newResponse;
        }

        private bool IsSwagger(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ToLowerInvariant().Contains("swagger");
        }
    }
}