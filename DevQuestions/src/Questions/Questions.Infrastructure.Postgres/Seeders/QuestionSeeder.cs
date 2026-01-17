using Shared.Database;

namespace Questions.Infrastructure.Postgres.Seeders;

public class QuestionSeeder : ISeeder
{
    private readonly QuestionsDbContext _dbContext;

    public QuestionSeeder(QuestionsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        await Task.Delay(500);

        // Implementation for seeding questions into the database
        throw new NotImplementedException();
    }
}