// Questions.Application

using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Shared.Abstractions;
using Shared.Extensions;

namespace Questions.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(instanceToValidate: request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(
                context: context,
                cancellation: cancellationToken)));

        var errors = validationResults.Where(result => !result.IsValid).ToList();

        if (errors.Count != 0)
        {
            // return failures.ToErrors();
            var failure = errors.ToErrors();

            var successType = typeof(TResponse).GetGenericArguments()[0];
            var failureType = typeof(TResponse).GetGenericArguments()[1];

            var failureMethod = typeof(Result)
                .GetMethods()
                .First(m => m.Name == "Failure" &&
                            m.GetParameters().Length == 1 &&
                            m.GetGenericArguments().Length == 2)
                .MakeGenericMethod(successType, failureType);

            return (TResponse)failureMethod.Invoke(null, [failure])!;
        }

        var response = await next(t: cancellationToken);

        return response;
    }
}