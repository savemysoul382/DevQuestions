using CSharpFunctionalExtensions;
using DevQuestions.Application.Questions;
using DevQuestions.Application.Questions.Fails;
using DevQuestions.Domain.Questions;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DevQuestions.Infrastructure.Postgres.Repositories;

public class QuestionsEfCoreRepository : IQuestionsRepository
{
    private readonly QuestionsDbContext _dbContext;

    public QuestionsEfCoreRepository(QuestionsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Question question, CancellationToken cancellationToken)
    {
        await _dbContext.Questions.AddAsync(entity: question, cancellationToken: cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken)
    {
        _dbContext.Questions.Attach(entity: question);
        await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken)
    {
        Question? question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken: cancellationToken);
        if (question == null) throw new ArgumentException("Question not found", nameof(questionId));

        _dbContext.Questions.Remove(entity: question);
        await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Result<Question?, Failure>> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        Question? question = await _dbContext.Questions
            .Include(q => q.Answers)
            .Include(q => q.Solution)
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken: cancellationToken);

        if (question is null)
        {
            return Errors.General.NotFound(id: questionId).ToFailure();
        }

        return question;
    }

    public async Task<int> GetOpenUserQuestionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions.CountAsync(q => q.UserId == userId && q.Status == QuestionStatus.OPEN, cancellationToken: cancellationToken);
    }
}