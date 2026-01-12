using DevQuestions.Application.Questions;
using DevQuestions.Domain.Questions;
using Microsoft.EntityFrameworkCore;

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
        _dbContext.Questions.Update(entity: question);
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

    public async Task<Question?> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        Question? question = await _dbContext.Questions
            .Include(q => q.Answers)
            .Include(q => q.Solution)
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken: cancellationToken);
        return question;
    }

    public async Task<int> GetOpenUserQuestionsASync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions.CountAsync(q => q.UserId == userId && q.Status == QuestionStatus.OPEN, cancellationToken: cancellationToken);
    }
}