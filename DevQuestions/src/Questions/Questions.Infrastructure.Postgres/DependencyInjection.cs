using Microsoft.Extensions.DependencyInjection;
using Questions.Application;

namespace Questions.Infrastructure.Postgres
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<QuestionsReadDbContext>();

            services.AddScoped<IQuestionsRepository, QuestionsEfCoreRepository>();

            return services;
        }
    }
}