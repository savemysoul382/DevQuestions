// DevQuestions.Application

using DevQuestions.Domain.Questions;

namespace DevQuestions.Application.Questions;

/// <summary>
/// Read - только для чтения.
/// </summary>
public interface IQuestionsReadDbContext
{
    IQueryable<Question> ReadQuestions { get; }
}