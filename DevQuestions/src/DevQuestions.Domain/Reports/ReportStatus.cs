// DevQuestions.Domain

namespace DevQuestions.Domain.Reports;

public enum ReportStatus
{
    /// <summary>
    ///  Статус открыт
    /// </summary>
    OPEN,

    /// <summary>
    /// Статус в рбаоте
    /// </summary>
    IN_PROGRESS,

    /// <summary>
    /// Статус решен
    /// </summary>
    RESOLVED,

    /// <summary>
    /// Статус закрыт
    /// </summary>
    DISMISSED,
}