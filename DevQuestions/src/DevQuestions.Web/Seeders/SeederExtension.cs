using DevQuestions.Infrastructure.Postgres;

namespace DevQuestions.Web.Seeders;

public static class SeederExtension
{
    public static async Task<WebApplication> UseSeedersAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IEnumerable<ISeeder> seeders = scope.ServiceProvider.GetServices<ISeeder>();
        foreach (ISeeder seeder in seeders)
        {
            await seeder.SeedAsync();
        }

        return app;
    }
}