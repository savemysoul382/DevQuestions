// DevQuestions.Domain

namespace DevQuestions.Domain.Questions;

public enum QuestionStatus
{
    /// <summary>
    /// Открыт
    /// </summary>
    OPEN,

    /// <summary>
    /// Закрыт
    /// </summary>
    RESOLVED,
}

public static class QuestionStatusExtensions
{
    public static string ToRussianString(this QuestionStatus status)
    {
        return status switch
        {
            QuestionStatus.OPEN => "Открыт",
            QuestionStatus.RESOLVED => "Решён",
            _ => "Неизвестный статус"
        };
    }
}