

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
       logger.LogInformation("[Start] Handle  {RequestName} with request: {@Request}", typeof(TRequest).Name, request);
       var timer = new Stopwatch();
         timer.Start();
        var response = next();
        timer.Stop();
        var timetaken = timer.ElapsedMilliseconds;
        if(timetaken > 1000)
        {
            logger.LogWarning("[Slow] Handle {RequestName} took {ElapsedMilliseconds} ms with request: {@Request}", typeof(TRequest).Name, timetaken, request);
        } else {
            logger.LogInformation("[End] Handle {RequestName} took {ElapsedMilliseconds} ms with request: {@Request}", typeof(TRequest).Name, timetaken, request);
        }
        return response;
    }
}
