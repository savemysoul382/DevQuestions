// DevQuestions.Application

using CSharpFunctionalExtensions;
using MediatR;

namespace Shared.Abstractions;

// public interface ICommand : IRequest<UnitResult<Failure>>;
// public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, UnitResult<Failure>>
//    where TCommand : ICommand;
public interface ICommand<TResponse> : IRequest<Result<TResponse, Failure>>;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse, Failure>>
    where TCommand : ICommand<TResponse>;