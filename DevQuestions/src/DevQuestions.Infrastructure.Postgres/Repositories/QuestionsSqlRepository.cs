using Dapper;
using DevQuestions.Application.Database;
using DevQuestions.Application.Questions;
using DevQuestions.Domain.Questions;

namespace DevQuestions.Infrastructure.Postgres.Repositories;

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
                Id = question.Id,
                Title = question.Title,
                Text = question.Text,
                UserId = question.UserId,
                Tags = question.Tags.ToArray(),
                Status = question.Status,
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

    public async Task<Question?> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);

        throw new NotImplementedException();
    }

    public async Task<int> GetOpenUserQuestionsASync(Guid userId, CancellationToken cancellationToken)
    {
        await Task.Delay(500);

        throw new NotImplementedException();
    }
}