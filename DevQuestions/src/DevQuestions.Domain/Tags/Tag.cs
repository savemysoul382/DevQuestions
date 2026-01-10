namespace DevQuestions.Domain.Tags;

public class Tag
{
    public Int32 Id { get; set; }

    public required String Name { get; set; }

    public required String Description { get; set; }
}