// DevQuestions.Application

using Questions.Domain;

namespace Questions.Application;

/// <summary>
/// Read - только для чтения.
/// </summary>
public interface IQuestionsReadDbContext
{
    IQueryable<Question> ReadQuestions { get; }
}