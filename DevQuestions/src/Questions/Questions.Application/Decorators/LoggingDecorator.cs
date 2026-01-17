// Questions.Application

using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;

namespace Questions.Application.Decorators;

public class LoggingDecorator<TResponse, TCommand> : ICommandHandler<TResponse, TCommand>
    where TCommand : ILogging
{
    private readonly ICommandHandler<TResponse, TCommand> _inner;
    private readonly ILogger<LoggingDecorator<TResponse, TCommand>> _logger;

    public LoggingDecorator(ICommandHandler<TResponse, TCommand> inner, ILogger<LoggingDecorator<TResponse, TCommand>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<TResponse, Failure>> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Command: {command}", command);

        var result = await _inner.HandleAsync(command: command, cancellationToken: cancellationToken);

        _logger.LogInformation("Command: {command}", command);

        return result;
    }
}