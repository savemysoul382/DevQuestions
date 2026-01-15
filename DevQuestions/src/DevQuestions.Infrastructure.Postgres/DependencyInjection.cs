using DevQuestions.Application.Questions;
using DevQuestions.Application.Tags;
using DevQuestions.Infrastructure.Postgres.Questions;
using DevQuestions.Infrastructure.Postgres.Tags;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Infrastructure.Postgres
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<QuestionsReadDbContext>();

            services.AddScoped<IQuestionsRepository, QuestionsEfCoreRepository>();
            services.AddScoped<IQuestionsReadDbContext, QuestionsReadDbContext>();
            services.AddScoped<ITagsReadDbContext, TagsReadDbContext>();

            return services;
        }
    }
}