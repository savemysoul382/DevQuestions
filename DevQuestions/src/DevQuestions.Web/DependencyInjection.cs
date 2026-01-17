using Infrastructure.ElasticSearch;
using Microsoft.AspNetCore.Mvc;
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
                .AddTagsModule();
        }

        private static IServiceCollection AddWebDependencies(this IServiceCollection services)
        {
            services.AddControllers();

            // пропускаем ошибки dto до контроллеров, иначе до метода они не доберутся
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

                // заменяет стандартную ошибку биндинга dto на свою
                // options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult("error");
            });

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