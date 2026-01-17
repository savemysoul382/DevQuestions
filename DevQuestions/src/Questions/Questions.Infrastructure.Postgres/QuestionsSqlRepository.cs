using CSharpFunctionalExtensions;
using Dapper;
using Questions.Application;
using Questions.Application.Features.GetQuestionsWithFilters;
using Questions.Domain;
using Shared;
using Shared.Database;

namespace Questions.Infrastructure.Postgres;

public class QuestionsSqlRepository : IQuestionsRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public QuestionsSqlRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Guid> AddAsync(Question question, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO questions (Id, Title, Content, UserId, Status)
                           VALUES (@Id, @Title, @Content, @UserId, @Status)
                           """;
        using var connection = _sqlConnectionFactory.Create();

        await connection.ExecuteAsync(
            sql,
            new
            {
                question.Id,
                question.Title,
                question.Text,
                question.UserId,
                Tags = question.Tags.ToArray(),
                question.Status,
            });

        return question.Id;
    }

    public async Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken)
    {
        await Task.Delay(500);
        throw new NotImplementedException();
    }

    public async Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);

        throw new NotImplementedException();
    }

    public async Task<Result<Question?, Failure>> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);

        throw new NotImplementedException();
    }

    public async Task<(IReadOnlyList<Question> Questions, long Count)> GetQuestionWithFiltersAsync(GetQuestionsWithFiltersQuery query, CancellationToken cancellationToken)
    {
        await Task.Delay(500);

        throw new NotImplementedException();
    }

    public async Task<int> GetOpenUserQuestionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);

        throw new NotImplementedException();
    }
}