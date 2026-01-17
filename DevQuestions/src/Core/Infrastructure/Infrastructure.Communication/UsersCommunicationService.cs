// DevQuestions.Infrastructure.Communication

using CSharpFunctionalExtensions;
using Shared;
using Shared.Communication;

namespace Infrastructure.Communication;

public class UsersCommunicationService : IUsersCommunicationService
{
    public async Task<Result<long, Failure>> GetUserRatingAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }
}