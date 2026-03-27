using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var commandName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("[Command] {CommandName} started", commandName);

        try
        {
            var response = await next();
            stopwatch.Stop();

            _logger.LogInformation("[Command] {CommandName} completed in {ElapsedMs}ms",
                commandName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex, "[Command] {CommandName} failed after {ElapsedMs}ms — {ExceptionType}: {Message}",
                commandName, stopwatch.ElapsedMilliseconds, ex.GetType().Name, ex.Message);

            throw;
        }
    }
}
