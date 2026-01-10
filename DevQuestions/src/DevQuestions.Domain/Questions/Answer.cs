// DevQuestions.Domain

namespace DevQuestions.Domain.Questions;

public class Answer
{
    public Answer(Guid user_id, String text, Question question)
    {
        UserId = user_id;
        Text = text;
        Question = question;
    }

    public Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public required String Text { get; set; }

    public required Question Question { get; set; }
    public required Answer Parent { get; set; }

    private List<Guid> Comments { get; set; } = [];

}