using CSharpFunctionalExtensions;
using DevQuestions.Application.Questions;
using DevQuestions.Application.Questions.Fails;
using DevQuestions.Application.Questions.Features.GetQuestionsWithFilters;
using DevQuestions.Domain.Questions;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DevQuestions.Infrastructure.Postgres.Repositories;

public class QuestionsEfCoreRepository : IQuestionsRepository
{
    private readonly QuestionsReadDbContext _readDbContext;

    public QuestionsEfCoreRepository(QuestionsReadDbContext readDbContext)
    {
        this._readDbContext = readDbContext;
    }

    public async Task<Guid> AddAsync(Question question, CancellationToken cancellationToken)
    {
        await this._readDbContext.Questions.AddAsync(entity: question, cancellationToken: cancellationToken);
        await this._readDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken)
    {
        this._readDbContext.Questions.Attach(entity: question);
        await this._readDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken)
    {
        Question? question = await this._readDbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken: cancellationToken);
        if (question == null) throw new ArgumentException("Question not found", nameof(questionId));

        this._readDbContext.Questions.Remove(entity: question);
        await this._readDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return question.Id;
    }

    public async Task<Result<Question?, Failure>> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        Question? question = await this._readDbContext.Questions
            .Include(q => q.Answers)
            .Include(q => q.Solution)
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken: cancellationToken);

        if (question is null)
        {
            return Errors.General.NotFound(id: questionId).ToFailure();
        }

        return question;
    }

    public async Task<(IReadOnlyList<Question> Questions, long Count)> GetQuestionWithFiltersAsync(GetQuestionsWithFiltersCommand command, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken: cancellationToken); // Simulate async work
        return (Questions: new List<Question>(), Count: 10);
    }

    public async Task<int> GetOpenUserQuestionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await this._readDbContext.Questions.CountAsync(q => q.UserId == userId && q.Status == QuestionStatus.OPEN, cancellationToken: cancellationToken);
    }
}