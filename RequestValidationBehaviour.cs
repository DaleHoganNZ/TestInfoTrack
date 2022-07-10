using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WebApplication.Core.Common.Behaviours
{
    public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;

        public RequestValidationBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // TODO: throw a validation exception if there are any validation errors
            // NOTE: the validation exception should contain all failures

            _logger.LogInformation($"Logging start time of {typeof(TRequest).Name} at {DateTime.Now.ToLongTimeString()}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var response = await next();
            stopWatch.Stop();
            _logger.LogInformation($"Logging end time of {typeof(TRequest).Name} at {DateTime.Now.ToLongTimeString()} - Duration was {stopWatch.ElapsedMilliseconds}ms");
            return response;
        }
    }
}
