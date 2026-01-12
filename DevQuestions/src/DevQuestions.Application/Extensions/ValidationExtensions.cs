using FluentValidation.Results;
using Shared;

namespace DevQuestions.Application.Extensions;

public static class ValidationExtensions
{
    public static Error[] ToErrors(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(e => Error.Validation(
            code: e.ErrorCode,
            message: e.ErrorMessage,
            invalidField: e.PropertyName)).ToArray();
    }
}