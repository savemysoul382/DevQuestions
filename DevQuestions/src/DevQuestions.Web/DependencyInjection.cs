using Infrastructure.ElasticSearch;
using Questions.Application;
using Questions.Infrastructure.Postgres;
using Questions.Presenters;
using Shared.FullTextSearch;

namespace DevQuestions.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
        {
            return services.AddQuestionsModule()
                .AddWebDependencies()
                .AddApplication()
                .AddPostgresInfrastructure()
                .AddSearchProvider();
        }

        private static IServiceCollection AddWebDependencies(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();

            return services;
        }

        private static IServiceCollection AddSearchProvider(this IServiceCollection services)
        {
            services.AddScoped<ISearchProvider, ElasticSearchProvider>();
            return services;
        }
    }
}