namespace DevQuestions.Domain.Questions;

public class Question
{
    public Guid Id { get; set; }

    public required String Title { get; set; } = String.Empty;

    public required String Text { get; set; } = String.Empty;

    public required Guid UserId { get; set; }

    public Guid? ScreenshotId { get; set; }

    List<Answer> Answers { get; set; } = [];

    public Answer? Solution { get; set; }

    private List<Guid> Tags { get; set; } = [];

}