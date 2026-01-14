namespace DevQuestions.Infrastructure.Postgres.Seeders;

public class QuestionSeeder : ISeeder
{
    private readonly QuestionsReadDbContext _readDbContext;

    public QuestionSeeder(QuestionsReadDbContext readDbContext)
    {
        this._readDbContext = readDbContext;
    }

    public async Task SeedAsync()
    {
        await Task.Delay(500);

        // Implementation for seeding questions into the database
        throw new NotImplementedException();
    }
}