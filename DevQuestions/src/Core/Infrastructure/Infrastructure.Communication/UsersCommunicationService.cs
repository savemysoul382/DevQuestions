// DevQuestions.Infrastructure.Communication

using CSharpFunctionalExtensions;
using DevQuestions.Application.Communication;
using Shared;

namespace DevQuestions.Infrastructure.Communication;

public class UsersCommunicationService : IUsersCommunicationService
{
    public async Task<Result<long, Failure>> GetUserRatingAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }
}