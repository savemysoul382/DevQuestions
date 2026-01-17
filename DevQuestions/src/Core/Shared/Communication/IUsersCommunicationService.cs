// DevQuestions.Application

using CSharpFunctionalExtensions;

namespace Shared.Communication;

public interface IUsersCommunicationService
{
    Task<Result<long, Failure>> GetUserRatingAsync(Guid userId, CancellationToken cancellationToken = default);
}