using FluentValidation.Results;

namespace Shared.Extensions;

public static class ValidationExtensions
{
    public static Failure ToErrors(this ValidationResult validationResult)
    {
        return validationResult.Errors.Select(e => Error.Validation(
            code: e.ErrorCode,
            message: e.ErrorMessage,
            invalidField: e.PropertyName)).ToArray();
    }
}