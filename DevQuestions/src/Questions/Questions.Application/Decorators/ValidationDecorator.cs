// Questions.Application

using CSharpFunctionalExtensions;
using FluentValidation;
using Shared;
using Shared.Abstractions;
using Shared.Extensions;

namespace Questions.Application.Decorators;

public class ValidationDecorator<TResponse, TCommand> : ICommandHandler<TResponse, TCommand>
    where TCommand : IValidation
{
    private readonly IEnumerable<IValidator> _validators;
    private readonly ICommandHandler<TResponse, TCommand> _inner;

    public ValidationDecorator(IEnumerable<IValidator<TCommand>> validators, ICommandHandler<TResponse, TCommand> inner)
    {
        _validators = validators;
        _inner = inner;
    }

    public async Task<Result<TResponse, Failure>> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await _inner.HandleAsync(command: command, cancellationToken: cancellationToken);
        }

        var context = new ValidationContext<TCommand>(instanceToValidate: command);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context: context, cancellation: cancellationToken)));

        var results = validationResults.Where(f => !f.IsValid).ToList();

        if (results.Count > 0)
        {
            return results.ToErrors();
        }

        // просто вызвать следующий обработчик в цепочке
        // return await _inner.HandleAsync(command: command, cancellationToken: cancellationToken);

        // или так
        var result = await _inner.HandleAsync(command: command, cancellationToken: cancellationToken);

        // логируем и тд
        return result;
    }
}