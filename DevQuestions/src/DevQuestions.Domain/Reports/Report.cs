namespace DevQuestions.Domain.Reports;

public class Report
{
    public int Id { get; set; }

    public required Guid UserId { get; set; }

    public required Guid ReportedUserId { get; set; }

    public Guid? ResolvedByUserId { get; set; }

    public required string Reason { get; set; }

    public Status Status { get; set; } = Status.OPEN;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public enum Status
{
    /// <summary>
    ///  Indicates that the operation or request is open and awaiting action.
    /// </summary>
    OPEN,

    /// <summary>
    /// Indicates that the operation is currently in progress.
    /// </summary>
    IN_PROGRESS,

    /// <summary>
    /// Indicates that an operation or request has been successfully completed and is in a resolved state.
    /// </summary>
    RESOLVED,

    /// <summary>
    /// Indicates that the item has been dismissed and is no longer active.
    /// </summary>
    DISMISSED,
}