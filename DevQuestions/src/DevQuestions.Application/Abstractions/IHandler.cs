// DevQuestions.Application

using CSharpFunctionalExtensions;
using Shared;

namespace DevQuestions.Application.Abstractions;

public interface ICommand;

public interface IHandler<TResponse, in TCommand>
    where TCommand : ICommand
{
    Task<Result<TResponse, Failure>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface IHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<Failure>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}