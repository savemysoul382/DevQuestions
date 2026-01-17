using Infrastructure.ElasticSearch;
using Questions.Presenters;
using Shared.FullTextSearch;
using Tags;

namespace DevQuestions.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
        {
            return services.AddWebDependencies()
                    .AddQuestionsModule()
                    .AddTagsModule()
                    .AddQuestionsModule();
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