using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Questions.Application;
using Questions.Application.Fails;
using Questions.Application.Features.GetQuestionsWithFilters;
using Questions.Domain;
using Shared;

namespace Questions.Infrastructure.Postgres;

public class QuestionsEfCoreRepository : IQuestionsRepository
{
    private readonly QuestionsReadDbContext _readDbContext;

    public QuestionsEfCoreRepository(QuestionsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Guid> AddAsync(Question question, CancellationToken cancellationToken)
    {
        await _readDbContext.Questions.AddAsync(entity: question, cancellationToken: cancellationToken);
        await _readDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken)
    {
        _readDbContext.Questions.Attach(entity: question);
        await _readDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken)
    {
        Question? question = await _readDbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken: cancellationToken);
        if (question == null) throw new ArgumentException("Question not found", nameof(questionId));

        _readDbContext.Questions.Remove(entity: question);
        await _readDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Result<Question?, Failure>> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        Question? question = await _readDbContext.Questions
            .Include(q => q.Answers)
            .Include(q => q.Solution)
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken: cancellationToken);

        if (question is null)
        {
            return Errors.General.NotFound(id: questionId).ToFailure();
        }

        return question;
    }

    public async Task<(IReadOnlyList<Question> Questions, long Count)> GetQuestionWithFiltersAsync(GetQuestionsWithFiltersQuery query, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken: cancellationToken); // Simulate async work
        return (Questions: new List<Question>(), Count: 10);
    }

    public async Task<int> GetOpenUserQuestionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _readDbContext.Questions.CountAsync(q => q.UserId == userId && q.Status == QuestionStatus.OPEN, cancellationToken: cancellationToken);
    }
}